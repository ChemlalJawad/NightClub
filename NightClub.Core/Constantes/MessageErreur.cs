using System;
using System.Collections.Generic;
using System.Text;

namespace NightClub.Core.Constantes
{
    public class MessageErreur
    {
        public static string RequeteNull => "L'intitulé du poste interne doit être renseigné";
        public static string DateBlacklistingInvalide => "La date de debut du blacklisting doit etre inférieur à la date de fin.";
        public static string DateFinBlacklistInferieur => "La date de fin du black listing doit etre superieur à la date d'aujourd'hui.";
        public static string MembreIntrouvable => "Le membre n'existe pas.";
        public static string MembreDejaBlackliste => "Le membre est déjà blacklisté.";
        public static string EmailEtTelephoneNonInvalide => "L'email ou le telephone ne peuvent etre vide.";
        public static string FormatEmailInvalide => "Le format de l'email est incorrecte.";
        public static string FormatTelephoneInvalide => "Le format du telephone est incorrecte";
        public static string FormatRegistreNationalInvalide => "Le format du registre national est incorrecte.";
        public static string CarteIdentiteInvalide => "Carte d'identité non trouvé, veuillez saisir un registre national correcte.";
        public static string NomNonReference=> "Nom ne peut pas etre vide";
        public static string PrenomNonReference => "Prenom ne peut pas etre vide.";
        public static string DateNaissanceNonReference => "DateNaissance ne peut pas etre vide.";
        public static string DateValidationeNonReference => "DateValidation ne peut pas etre vide.";
        public static string DateExpirationNonReference => "DateExpiration ne peut pas etre vide.";
        public static string DateValidationInvalide => "La date de validation de votre carte d'identité doit etre inferieur à la date d'expiration.";
        public static string CarteIdentiteExpire => "Votre carte d'identité est expiré.";
        public static string AgeMinimumRequis => "Il faut avoir 18 ans minimum pour pouvoir s'enregistrer.";
        public static string RegistreNationalDejaExistant => "Ce numéro de registre national est déjà dans le systeme.";
    }
}
