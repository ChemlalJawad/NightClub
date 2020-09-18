using Microsoft.EntityFrameworkCore;
using NightClub.Core.Constantes;
using NightClub.Core.Domaines;
using NightClub.Core.Exceptions;
using NightClub.Data.Database;
using NightClub.Service.Membre.Requete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace NightClub.Service.Membre
{
    public class MembreService : IMembreService
    {
        private readonly NightclubContext _context;
        private const int AgeMinimum = 18;
        private DateTime defaultDate = new DateTime();
        public MembreService(NightclubContext context)
        {
            _context = context;
        }

        public Core.Domaines.Membre BlacklisterMembre(BlacklisterMembreRequete requete)
        {
            if (requete == null)
            {
                throw new CustomBadRequestException(MessageErreur.RequeteNull);
            }

            if (DateTime.Compare(requete.DebutDateBlacklister, requete.FinDateBlacklister) > 0)
            {
                throw new CustomBadRequestException(MessageErreur.DateBlacklistingInvalide);
            }

            if (DateTime.Compare(requete.FinDateBlacklister, DateTime.Today) < 0)
            {
                throw new CustomBadRequestException(MessageErreur.DateFinBlacklistInferieur);
            }

            var membreBlacklister = _context.Membres.SingleOrDefault(x => x.Id == requete.MembreId);
            if (membreBlacklister == null)
            {
                throw new CustomNotFoundException(MessageErreur.MembreIntrouvable);
            }
            if (membreBlacklister.IsBlacklister)
            {
                throw new CustomBadRequestException(MessageErreur.MembreDejaBlackliste);
            }

            membreBlacklister.IsBlacklister = true;
            membreBlacklister.DebutDateBlacklister = requete.DebutDateBlacklister;
            membreBlacklister.FinDateBlacklister = requete.FinDateBlacklister;

            _context.Membres.Update(membreBlacklister);
            _context.SaveChanges();
            return membreBlacklister;
        }

        public Core.Domaines.Membre CreerMembre(CreerMembreRequete requete)
        {
            if (requete == null )
            {
                throw new CustomBadRequestException(MessageErreur.RequeteNull);
            }

            if (String.IsNullOrEmpty(requete.Telephone) && String.IsNullOrEmpty(requete.Email))
            {
                throw new CustomBadRequestException(MessageErreur.EmailEtTelephoneNonInvalide);
            }

            if (!String.IsNullOrEmpty(requete.Email))
            {
                string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
                var regexMatch = Regex.Match(requete.Email, pattern);
                if (!regexMatch.Success) throw new CustomBadRequestException(MessageErreur.FormatEmailInvalide);
            }

            if (!String.IsNullOrEmpty(requete.Telephone))
            {
                string pattern = @"^0[0-9]{9}$";
                var regexMatch = Regex.Match(requete.Telephone, pattern);
                if (!regexMatch.Success) throw new CustomBadRequestException(MessageErreur.FormatTelephoneInvalide);
            }
            var idCarteExistant = _context.IDCartes.SingleOrDefault(x => x.RegistreNational == requete.CarteIdentite.RegistreNational);
            if (idCarteExistant != null) throw new CustomNotFoundException(MessageErreur.RegistreNationalDejaExistant);

            var nouvelleIDCarte = new IDCarte
            {
                Nom = requete.CarteIdentite.Nom,
                Prenom = requete.CarteIdentite.Nom,
                DateNaissance = requete.CarteIdentite.DateNaissance,
                RegistreNational = requete.CarteIdentite.RegistreNational,
                DateValidation = requete.CarteIdentite.DateValidation,
                DateExpiration = requete.CarteIdentite.DateExpiration,
                NumeroCarte = requete.CarteIdentite.NumeroCarte
            };
            ValidationCarteIdentite(nouvelleIDCarte);
          
            var nouvelleCarteMembre = new MembreCarte
            {
                Code = Guid.NewGuid().ToString(),
                IsActive = true
            };

            var nouveauMembre = new Core.Domaines.Membre
            {
                Email = requete.Email,
                Telephone = requete.Telephone,
                IsBlacklister = false,
                DebutDateBlacklister = new DateTime(),
                FinDateBlacklister = new DateTime(),
                CarteIdentites = new List<IDCarte>() { nouvelleIDCarte },
                MembreCartes = new List<MembreCarte>() { nouvelleCarteMembre }
            };

            _context.Membres.Add(nouveauMembre);
            _context.SaveChanges();
            return nouveauMembre;
        }

        public Core.Domaines.Membre ModifierMembre(ModifierMembreRequete requete)
        {
            if (requete == null)
            {
                throw new CustomBadRequestException(MessageErreur.RequeteNull);
            }

            var membre = _context.Membres
                .Include(x => x.CarteIdentites)
                .SingleOrDefault(x => x.Id == requete.MembreId);
            if (membre == null) throw new CustomNotFoundException(MessageErreur.MembreIntrouvable);

            if (!String.IsNullOrEmpty(requete.Email)) membre.Email = requete.Email;
            if (!String.IsNullOrEmpty(requete.Telephone)) membre.Telephone = requete.Telephone;
           
            var nouvelleCarteID = _context.IDCartes.SingleOrDefault(x => x.RegistreNational == requete.CarteIdentite.RegistreNational);
            if (nouvelleCarteID == null) throw new CustomNotFoundException(MessageErreur.CarteIdentiteInvalide);
            // Si un element non vide dans le body de la requete , on lui affecte une nouvelle valeur
            if (!String.IsNullOrEmpty(requete.CarteIdentite.Nom)) nouvelleCarteID.Nom = requete.CarteIdentite.Nom;
            if (!String.IsNullOrEmpty(requete.CarteIdentite.Prenom)) nouvelleCarteID.Prenom = requete.CarteIdentite.Prenom;
            if (requete.CarteIdentite.DateNaissance != defaultDate) nouvelleCarteID.DateNaissance = requete.CarteIdentite.DateNaissance;
            if (requete.CarteIdentite.DateValidation != defaultDate) nouvelleCarteID.DateValidation = requete.CarteIdentite.DateValidation;
            if (requete.CarteIdentite.DateExpiration != defaultDate) nouvelleCarteID.DateExpiration = requete.CarteIdentite.DateExpiration;
            if (requete.CarteIdentite.NumeroCarte != default) nouvelleCarteID.NumeroCarte = requete.CarteIdentite.NumeroCarte;

            ValidationCarteIdentite(nouvelleCarteID);
            membre.CarteIdentites.Add(nouvelleCarteID);
            
            _context.Membres.Update(membre);
            _context.SaveChanges();
            return membre;
        }

        public Core.Domaines.Membre GenerNouvelleCarteMembre(int membreId)
        {
            var membre = _context.Membres
                .Include(x => x.MembreCartes)
                .SingleOrDefault(x => x.Id == membreId);
            if (membre == null) throw new CustomNotFoundException(MessageErreur.MembreIntrouvable);

            foreach(var carteMembre in membre.MembreCartes)
            {
                if (carteMembre.IsActive) 
                {
                    carteMembre.IsActive = false;
                }
            }

            membre.MembreCartes.Add(
                new MembreCarte
                {
                    Code = Guid.NewGuid().ToString(),
                    IsActive = true
                });

            _context.SaveChanges();
            return membre;
        }

        private void ValidationCarteIdentite(IDCarte idCarte)
        {
            if (!String.IsNullOrEmpty(idCarte.RegistreNational))
            {
                string NissPattern = @"^[0-9]{2}.[0-9]{2}.[0-9]{2}-[0-9]{3}.[0-9]{2}$";
                var regexMatch = Regex.Match(idCarte.RegistreNational, NissPattern);
                if (!regexMatch.Success) throw new CustomBadRequestException(MessageErreur.FormatRegistreNationalInvalide);
            }

            if (String.IsNullOrEmpty(idCarte.Nom))
            {
                throw new CustomBadRequestException(MessageErreur.NomNonReference);
            }

            if (String.IsNullOrEmpty(idCarte.Prenom))
            {
                throw new CustomBadRequestException(MessageErreur.PrenomNonReference);
            }

            if (idCarte.DateNaissance == defaultDate)
            {
                throw new CustomBadRequestException(MessageErreur.DateNaissanceNonReference);
            }

            if (idCarte.DateValidation == defaultDate)
            {
                throw new CustomBadRequestException(MessageErreur.DateValidationeNonReference);
            }

            if (idCarte.DateExpiration == defaultDate)
            {
                throw new CustomBadRequestException(MessageErreur.DateExpirationNonReference);
            }

            if (DateTime.Compare(idCarte.DateExpiration, idCarte.DateValidation) < 0)
            {
                throw new CustomBadRequestException(MessageErreur.DateValidationInvalide);
            }

            if (DateTime.Compare(idCarte.DateExpiration, DateTime.Today) < 0)
            {
                throw new CustomBadRequestException(MessageErreur.CarteIdentiteExpire);
            }

            if (idCarte.CalculerAge(idCarte.DateNaissance) < AgeMinimum)
            {
                throw new CustomBadRequestException(MessageErreur.AgeMinimumRequis);
            }
        }

        public List<Core.Domaines.Membre> RecupererMembreBlacklister()
        {
            var blacklistMembres = _context.Membres.ToList();
            blacklistMembres = blacklistMembres.Where(x => x.IsBlacklister).ToList();

            return blacklistMembres;
        }
    }
}
