using Microsoft.EntityFrameworkCore;
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
        public MembreService(NightclubContext context)
        {
            _context = context;
        }

        public Core.Domaines.Membre BlacklisterMembre(BlacklisterMembreRequete requete)
        {
            if (requete == null)
            {
                throw new CustomBadRequestException("Requete est nulle");
            }

            if (DateTime.Compare(requete.DebutDateBlacklister, requete.FinDateBlacklister) > 0)
            {
                throw new CustomBadRequestException("periode de bannissement invalide");
            }

            if (DateTime.Compare(requete.FinDateBlacklister, DateTime.Today) < 0)
            {
                throw new CustomBadRequestException("La date de fin du blacklist doit etre superieur a la date d'ajd");
            }

            var membreBlacklister = _context.Membres.SingleOrDefault(x => x.Id == requete.MembreId);
            if (membreBlacklister == null)
            {
                throw new CustomNotFoundException("Le membre en question n'existe pas");
            }
            if (membreBlacklister.IsBlacklister)
            {
                throw new CustomBadRequestException("Le membre est deja blacklisté");
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
                throw new CustomBadRequestException("Requete est nulle");
            }

            if (String.IsNullOrEmpty(requete.Telephone) && String.IsNullOrEmpty(requete.Email))
            {
                throw new CustomBadRequestException("Email et telephone non specifié");
            }

            if (!String.IsNullOrEmpty(requete.Email))
            {
                string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
                var regexMatch = Regex.Match(requete.Email, pattern);
                if (!regexMatch.Success) throw new CustomBadRequestException("email invalide");
            }

            if (!String.IsNullOrEmpty(requete.Telephone))
            {
                string pattern = @"/^((\+|00)32\s?|0)4(60|[789]\d)(\s?\d{2}){3}$/";
                var regexMatch = Regex.Match(requete.Telephone, pattern);
                if (!regexMatch.Success) throw new CustomBadRequestException("Telephone invalide");
            }
            
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
                throw new CustomBadRequestException("Requete est nulle");
            }

            var membre = _context.Membres
                .Include(x => x.CarteIdentites)
                .SingleOrDefault(x => x.Id == requete.MembreId);
            if (membre == null) throw new CustomNotFoundException("membre existe pas ");

            if (!String.IsNullOrEmpty(requete.Email)) membre.Email = requete.Email;
            if (!String.IsNullOrEmpty(requete.Telephone)) membre.Telephone = requete.Telephone;
           

            var nouvelleCarteID = _context.IDCartes.SingleOrDefault(x => x.RegistreNational == requete.CarteIdentite.RegistreNational);
            if (nouvelleCarteID == null) throw new CustomNotFoundException("L'id carte n'existe pas");

            if (!String.IsNullOrEmpty(requete.CarteIdentite.Nom)) nouvelleCarteID.Nom = requete.CarteIdentite.Nom;
            if (!String.IsNullOrEmpty(requete.CarteIdentite.Prenom)) nouvelleCarteID.Prenom = requete.CarteIdentite.Prenom;
            if (requete.CarteIdentite.DateNaissance != new DateTime()) nouvelleCarteID.DateNaissance = requete.CarteIdentite.DateNaissance;
            if (requete.CarteIdentite.DateValidation != new DateTime()) nouvelleCarteID.DateValidation = requete.CarteIdentite.DateValidation;
            if (requete.CarteIdentite.DateExpiration != new DateTime()) nouvelleCarteID.DateExpiration = requete.CarteIdentite.DateExpiration;
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
            if (membre == null) throw new CustomNotFoundException("membre n'existe pas");

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
            if(!String.IsNullOrEmpty(idCarte.RegistreNational))
            {
                string NissPattern = @"^\d{3}.\d{2}.\d{2}-\d{3}-\d{2}$";
                var regexMatch = Regex.Match(idCarte.RegistreNational, NissPattern);
                if( regexMatch.Success) throw new CustomBadRequestException("Format Registre national invalide");
            }

            if (String.IsNullOrEmpty(idCarte.Nom))
            {
                throw new CustomBadRequestException("Nom invalide");
            }

            if (idCarte.DateNaissance == null)
            {
                throw new CustomBadRequestException("Date de naissance invalide");
            }

            if (idCarte.DateValidation == null)
            {
                throw new CustomBadRequestException("Date de naissance invalide");
            }

            if (idCarte.DateExpiration == null)
            {
                throw new CustomBadRequestException("Date de naissance invalide");
            }

            if (String.IsNullOrEmpty(idCarte.Prenom))
            {
                throw new CustomBadRequestException("Prenom invalide");
            }

            if (DateTime.Compare(idCarte.DateExpiration, idCarte.DateValidation) < 0)
            {
                throw new CustomBadRequestException("Date de validation ne peut pas etre superieur a date d expiration");
            }

            if (DateTime.Compare(idCarte.DateExpiration, DateTime.Today) < 0)
            {
                throw new CustomBadRequestException("Date de la carte identité expiré");
            }

            if (idCarte.GetAge(idCarte.DateNaissance) < AgeMinimum)
            {
                throw new CustomBadRequestException("Pas 18 ans");
            }
        }

    }
}
