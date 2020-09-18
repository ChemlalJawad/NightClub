# NightClub
Project Boite de nuit
# Introduction

Nightclub was built from an ASP.NET WEB Project with a JSON API.

# API Request 

Method | URL | Details
----|-------------------- | -------------------------
GET| /api/membres | Affiche tout les membres blacklistés
POST| /api/membres | Permet de créer un nouveau membre
POST| /api/{membreId}/blacklist| Permet de blacklister un membre
POST| /api/{membreId}/modifierMembre| Modifier les informations d'un membre
POST| /api/membreId}/nouvelleCarteMembre| Permet de generer une nouvelle carte de membre


Exemple de body pour la création d'un membre
{
  {
    "email" : "Test@hotmail.com",
    "CarteIdentite" : {
        "Prenom" : "Test",
        "Nom" : "Test",
        "RegistreNational" : "11.11.11-111.11",
        "DateNaissance" : "2001-01-01",
        "DateValidation" : "2031-01-01",
        "DateExpiration" : "2020-01-01",
        "NumeroCarte" : 1111111
    }
}
