using AutoFixture;
using Microsoft.EntityFrameworkCore;
using NightClub.Data.Database;
using NightClub.Service.Membre;
using System;
using FluentAssertions;
using Xunit;
using NightClub.Core.Exceptions;
using NightClub.Core.Constantes;
using NightClub.Service.Membre.Requete;
using NightClub.Core.Domaines;
using System.Collections.Generic;
using System.Linq;

namespace NightClub.Test
{
    public class MembreTest : TestingContext<MembreService>
    {
        public MembreTest()
        {
            base.Setup();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
        private NightclubContext InitializeContext()
        {
            var options = new DbContextOptionsBuilder<NightclubContext>()
              .UseInMemoryDatabase(databaseName: "NightclubTest")
              .Options;

            var context = new NightclubContext(options);
            context.Database.EnsureDeleted();

            context.SaveChanges();
            return context;
        }

        [Fact]
        public void CreerMembre_EmailEtTelException()
        {
            // Arrange
            var requete = new CreerMembreRequete() { };

            // Act
            Action action = () => ClassUnderTest.CreerMembre(requete);

            // Assert
            action
                .Should()
                .ThrowExactly<CustomBadRequestException>()
                .WithMessage(MessageErreur.EmailEtTelephoneNonInvalide);
        }

        [Fact]
        public void CreerMembre_EmailFormatException()
        {
            // Arrange
            var requete = new CreerMembreRequete() { Email = "jade" };

            // Act
            Action action = () => ClassUnderTest.CreerMembre(requete);

            // Assert
            action
                .Should()
                .ThrowExactly<CustomBadRequestException>()
                .WithMessage(MessageErreur.FormatEmailInvalide);
        }

        [Fact]
        public void CreerMembre_TelephoneFormatException()
        {
            // Arrange
            var requete = new CreerMembreRequete() { Telephone = "01" };

            // Act
            Action action = () => ClassUnderTest.CreerMembre(requete);

            // Assert
            action
                .Should()
                .ThrowExactly<CustomBadRequestException>()
                .WithMessage(MessageErreur.FormatTelephoneInvalide);
        }

        [Fact]
        public void CreerMembre_IDCarteRegistreNationalException()
        {
            // Arrange
            var idCarte = new CreerMembreRequete.IDCarte 
            { 
                Nom = "Test",
                Prenom = "Test",
                DateExpiration = new DateTime(2030, 7, 29),
                DateValidation = new DateTime(2017, 7, 29),
                DateNaissance = new DateTime(1995, 7, 29),
                RegistreNational = "123" ,
                NumeroCarte = 1111
            };

            var requete = new CreerMembreRequete() 
            { 
                Email = "chafik@hotmail.com", 
                CarteIdentite =  idCarte  
            };

            // Act
            Action action = () => ClassUnderTest.CreerMembre(requete);

            // Assert
            action
                .Should()
                .ThrowExactly<CustomBadRequestException>()
                .WithMessage(MessageErreur.FormatRegistreNationalInvalide);
        }

        [Fact]
        public void CreerMembre_DateNaissanceNonReferenceException()
        {
            // Arrange
            var idCarte = new CreerMembreRequete.IDCarte
            {
                Nom = "Test",
                Prenom = "Test",
                DateExpiration = new DateTime(2030, 7, 29),
                DateValidation = new DateTime(2017, 7, 29),
                RegistreNational = "95.07.29-137.67",
                NumeroCarte = 1111
            };

            var requete = new CreerMembreRequete()
            {
                Email = "chafik@hotmail.com",
                CarteIdentite = idCarte
            };

            // Act
            Action action = () => ClassUnderTest.CreerMembre(requete);

            // Assert
            action
                .Should()
                .ThrowExactly<CustomBadRequestException>()
                .WithMessage(MessageErreur.DateNaissanceNonReference);
        }

        [Fact]
        public void CreerMembre_DateValidationNonReferenceException()
        {
            // Arrange
            var idCarte = new CreerMembreRequete.IDCarte
            {
                Nom = "Test",
                Prenom = "Test",
                DateExpiration = new DateTime(2030, 7, 29),
                DateNaissance = new DateTime(1995, 7, 29),
                RegistreNational = "95.07.29-137.67",
                NumeroCarte = 1111
            };

            var requete = new CreerMembreRequete()
            {
                Email = "chafik@hotmail.com",
                CarteIdentite = idCarte
            };

            // Act
            Action action = () => ClassUnderTest.CreerMembre(requete);

            // Assert
            action
                .Should()
                .ThrowExactly<CustomBadRequestException>()
                .WithMessage(MessageErreur.DateValidationeNonReference);
        }

        [Fact]
        public void CreerMembre_DateExpirationNonReferenceException()
        {
            // Arrange
            var idCarte = new CreerMembreRequete.IDCarte
            {
                Nom = "Test",
                Prenom = "Test",
                DateValidation = new DateTime(2018, 7, 29),
                DateNaissance = new DateTime(1995, 7, 29),
                RegistreNational = "95.07.19-111.26",
                NumeroCarte = 1111
            };

            var requete = new CreerMembreRequete()
            {
                Email = "chafik@hotmail.com",
                CarteIdentite = idCarte
            };

            // Act
            Action action = () => ClassUnderTest.CreerMembre(requete);

            // Assert
            action
                .Should()
                .ThrowExactly<CustomBadRequestException>()
                .WithMessage(MessageErreur.DateExpirationNonReference);
        }

        [Fact]
        public void CreerMembre_DateValidationSuperieurDateExpirationException()
        {
            // Arrange
            var idCarte = new CreerMembreRequete.IDCarte
            {
                Nom = "Test",
                Prenom = "Test",
                DateValidation = new DateTime(2018, 7, 29),
                DateNaissance = new DateTime(1995, 7, 29),
                DateExpiration = new DateTime(2009, 7, 29),
                RegistreNational = "95.07.19-111.26",
                NumeroCarte = 1111
            };

            var requete = new CreerMembreRequete()
            {
                Email = "test@hotmail.com",
                CarteIdentite = idCarte
            };

            // Act
            Action action = () => ClassUnderTest.CreerMembre(requete);

            // Assert
            action
                .Should()
                .ThrowExactly<CustomBadRequestException>()
                .WithMessage(MessageErreur.DateValidationInvalide);
        }

        [Fact]
        public void CreerMembre_CarteIdentiteExpireException()
        {
            // Arrange
            var idCarte = new CreerMembreRequete.IDCarte
            {
                Nom = "Test",
                Prenom = "Test",
                DateValidation = new DateTime(2018, 7, 29),
                DateNaissance = new DateTime(1995, 7, 29),
                DateExpiration = new DateTime(2020, 7, 29),
                RegistreNational = "95.07.19-111.26",
                NumeroCarte = 1111
            };

            var requete = new CreerMembreRequete()
            {
                Email = "test@hotmail.com",
                CarteIdentite = idCarte
            };

            // Act
            Action action = () => ClassUnderTest.CreerMembre(requete);

            // Assert
            action
                .Should()
                .ThrowExactly<CustomBadRequestException>()
                .WithMessage(MessageErreur.CarteIdentiteExpire);
        }

        [Fact]
        public void CreerMembre_AgeMinimumException()
        {
            // Arrange
            var idCarte = new CreerMembreRequete.IDCarte
            {
                Nom = "Test",
                Prenom = "Test",
                DateValidation = new DateTime(2018, 7, 29),
                DateNaissance = new DateTime(2004, 7, 29),
                DateExpiration = new DateTime(2030, 7, 29),
                RegistreNational = "95.07.19-111.26",
                NumeroCarte = 1111
            };

            var requete = new CreerMembreRequete()
            {
                Email = "test@hotmail.com",
                CarteIdentite = idCarte
            };

            // Act
            Action action = () => ClassUnderTest.CreerMembre(requete);

            // Assert
            action
                .Should()
                .ThrowExactly<CustomBadRequestException>()
                .WithMessage(MessageErreur.AgeMinimumRequis);
        }

        [Fact]
        public void CreerMembre_Success()
        {
            // Arrange
            var context = InitializeContext();
            var idCarte = new CreerMembreRequete.IDCarte
            {
                Nom = "Test",
                Prenom = "Test",
                DateValidation = new DateTime(2018, 7, 29),
                DateNaissance = new DateTime(2000, 7, 29),
                DateExpiration = new DateTime(2030, 7, 29),
                RegistreNational = "95.07.19-111.26",
                NumeroCarte = 1111
            };

            var requete = new CreerMembreRequete()
            {
                Email = "test@hotmail.com",
                CarteIdentite = idCarte
            };
            InjectClassFor(context);
            // Act
            var result = ClassUnderTest.CreerMembre(requete);

            // Assert
            result
                .Email
                .Should()
                .Be("test@hotmail.com");
            result
                .CarteIdentites
                .First()
                .RegistreNational
                .Should()
                .Be("95.07.19-111.26");
        }


    }
}
