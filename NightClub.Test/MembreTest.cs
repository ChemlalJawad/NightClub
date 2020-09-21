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
            var context = InitializeContext();
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
            InjectClassFor(context);

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
            var context = InitializeContext();
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
            InjectClassFor(context);

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
            var context = InitializeContext();
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
            InjectClassFor(context);

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
            var context = InitializeContext();
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
            InjectClassFor(context);

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
            var context = InitializeContext();
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
            InjectClassFor(context);

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
            var context = InitializeContext();
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
            InjectClassFor(context);

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
            var context = InitializeContext();
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
            InjectClassFor(context);

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

        [Fact]
        public void BlacklisterMembre_DateDebutBlacklistInferieurDateFinBlacklistException()
        {
            // Arrange
            var context = InitializeContext();
            var membre = fixture.Build<Membre>().With(x => x.Id, 15).With(x => x.IsBlacklister, false).Create();
            var requete = new BlacklisterMembreRequete
            {
                MembreId = 15,
                DebutDateBlacklister = new DateTime(2030, 10, 22),
                FinDateBlacklister = new DateTime(2030, 7, 29)
             
            };
            context.Membres.Add(membre);
            InjectClassFor(context);

            // Act
            Action action = () => 
            ClassUnderTest.BlacklisterMembre(requete);

            // Assert
            action
                .Should()
                .ThrowExactly<CustomBadRequestException>()
                .WithMessage(MessageErreur.DateBlacklistingInvalide);
        }


        [Fact]
        public void BlacklisterMembre_DateDFinBlacklistInferieurDateDuJourException()
        {
            // Arrange
            var context = InitializeContext();
            var membre = fixture.Build<Membre>().With(x => x.Id, 10).With(x => x.IsBlacklister, false).Create();
            var requete = new BlacklisterMembreRequete
            {
                MembreId = 10,
                DebutDateBlacklister = new DateTime(2019, 01, 22),
                FinDateBlacklister = new DateTime(2020, 7, 29)

            };
            context.Membres.Add(membre);
            InjectClassFor(context);

            // Act
            Action action = () => ClassUnderTest.BlacklisterMembre(requete);

            // Assert
            action
                .Should()
                .ThrowExactly<CustomBadRequestException>()
                .WithMessage(MessageErreur.DateFinBlacklistInferieur);
        }
        [Fact]
        public void BlacklisterMembre_MembreInexistantException()
        {
            // Arrange
            var context = InitializeContext();
            var requete = new BlacklisterMembreRequete
            {
                MembreId = 1,
                DebutDateBlacklister = new DateTime(2020, 10, 22),
                FinDateBlacklister = new DateTime(2021, 7, 29)

            };
            InjectClassFor(context);

            // Act
            Action action = () => ClassUnderTest.BlacklisterMembre(requete);

            // Assert
            action
                .Should()
                .ThrowExactly<CustomNotFoundException>()
                .WithMessage(MessageErreur.MembreIntrouvable);
        }

        // En debug --> donne le bon resultat 
        [Fact]
        public void BlacklisterMembre_DejaBlacklisterException()
        {
            // Arrange
            var context = InitializeContext();
            var membre = fixture.Build<Membre>().With(x => x.Id, 1).With(x => x.IsBlacklister, true).Create();
            var requete = new BlacklisterMembreRequete
            {
                MembreId = 1,
                DebutDateBlacklister = new DateTime(2020, 10, 22),
                FinDateBlacklister = new DateTime(2021, 7, 29)

            };
            context.Membres.Add(membre);
            context.SaveChanges();
            InjectClassFor(context);

            // Act
            Action action = () => ClassUnderTest.BlacklisterMembre(requete);

            // Assert
            action
                .Should()
                .ThrowExactly<CustomBadRequestException>()
                .WithMessage(MessageErreur.MembreDejaBlackliste);
        }

        [Fact]
        public void BlacklisterMembre_Success()
        {
            // Arrange
            var context = InitializeContext();
            var membre = fixture.Build<Membre>().With(x => x.Id, 3).With(x => x.IsBlacklister, false).Create();
            var requete = new BlacklisterMembreRequete
            {
                MembreId = 3,
                DebutDateBlacklister = new DateTime(2020, 10, 22),
                FinDateBlacklister = new DateTime(2021, 7, 29)

            };
            context.Membres.Add(membre);
            context.SaveChanges();
            InjectClassFor(context);

            // Act
            var result = ClassUnderTest.BlacklisterMembre(requete);

            // Assert
            result.Id
                .Should()
                .Be(3);

            result.IsBlacklister
                .Should()
                .Be(true);

            result.FinDateBlacklister
               .Should()
               .Be(new DateTime(2021, 7, 29));
        }

    }
}
