using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace MVS.Common.Models
{
    public partial class mvsrecetteContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public mvsrecetteContext()
        {
        }

        public mvsrecetteContext(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public mvsrecetteContext(DbContextOptions<mvsrecetteContext> options, IConfiguration configuration)
            : base(options)
        {
            this._configuration = configuration;
        }

        public mvsrecetteContext(DbContextOptions<mvsrecetteContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<CounterVaultCreate> CounterVaultCreates { get; set; }
        public virtual DbSet<JobParticular> JobParticulars { get; set; }
        public virtual DbSet<JobProfessionel> JobProfessionels { get; set; }
        public virtual DbSet<Vault> Vaults { get; set; }
        public virtual DbSet<VaultAdministrativeLife> VaultAdministrativeLives { get; set; }
        public virtual DbSet<VaultAnswersAnticipationMeasure> VaultAnswersAnticipationMeasures { get; set; }
        public virtual DbSet<VaultAnswersDigitalLife> VaultAnswersDigitalLives { get; set; }
        public virtual DbSet<VaultAnswersHeritage> VaultAnswersHeritages { get; set; }
        public virtual DbSet<VaultAnswersPersonal> VaultAnswersPersonals { get; set; }
        public virtual DbSet<VaultAnticipationMeasuresInfo> VaultAnticipationMeasuresInfos { get; set; }
        public virtual DbSet<VaultCategory> VaultCategories { get; set; }
        public virtual DbSet<VaultContact> VaultContacts { get; set; }
        public virtual DbSet<VaultContract> VaultContracts { get; set; }
        public virtual DbSet<VaultDigitalLife> VaultDigitalLives { get; set; }
        public virtual DbSet<VaultDocument> VaultDocuments { get; set; }
        public virtual DbSet<VaultFamilyInfo> VaultFamilyInfos { get; set; }
        public virtual DbSet<VaultFuneraryVolonte> VaultFuneraryVolontes { get; set; }
        public virtual DbSet<VaultHeritage> VaultHeritages { get; set; }
        public virtual DbSet<VaultPersonalInfo> VaultPersonalInfos { get; set; }
        public virtual DbSet<VaultTiersContact> VaultTiersContacts { get; set; }
        public virtual DbSet<VaultUser> VaultUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(this._configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.Comment)
                    .HasMaxLength(2048)
                    .HasComment("Champ libre pour compléments");

                entity.Property(e => e.Data).HasMaxLength(2048);

                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasComment("Id user répond à la question");

                entity.Property(e => e.VaultId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasComment("Id bénéficiaire/dossier pour lequel répond");

                entity.HasOne(d => d.JobNavigation)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.Job)
                    .HasConstraintName("FK_Answers_JobParticular");
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(350);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(350);

                entity.Property(e => e.MutacNumber).HasMaxLength(450);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<CounterVaultCreate>(entity =>
            {
                entity.ToTable("CounterVaultCreate");
            });

            modelBuilder.Entity<JobParticular>(entity =>
            {
                entity.ToTable("JobParticular");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Job).IsRequired();
            });

            modelBuilder.Entity<JobProfessionel>(entity =>
            {
                entity.ToTable("JobProfessionel");

                entity.Property(e => e.Job).IsRequired();
            });

            modelBuilder.Entity<Vault>(entity =>
            {
                entity.Property(e => e.BirthName).HasMaxLength(450);

                entity.Property(e => e.IsArchived).HasColumnName("isArchived");

                entity.Property(e => e.Nationality).HasMaxLength(450);

                entity.Property(e => e.Title).IsRequired();

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Vaults)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vaults_AspNetUsers");
            });

            modelBuilder.Entity<VaultAdministrativeLife>(entity =>
            {
                entity.ToTable("VaultAdministrativeLife");

                entity.Property(e => e.VaultId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Vault)
                    .WithMany(p => p.VaultAdministrativeLives)
                    .HasForeignKey(d => d.VaultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VaultAdministrativeLife_Vaults");
            });

            modelBuilder.Entity<VaultAnswersAnticipationMeasure>(entity =>
            {
                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.VaultId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.JobNavigation)
                    .WithMany(p => p.VaultAnswersAnticipationMeasures)
                    .HasForeignKey(d => d.Job)
                    .HasConstraintName("FK_VaultAnswersAnticipationMeasures_JobParticular");

                entity.HasOne(d => d.Vault)
                    .WithMany(p => p.VaultAnswersAnticipationMeasures)
                    .HasForeignKey(d => d.VaultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VaultAnswersAnticipationMeasures_Vaults");
            });

            modelBuilder.Entity<VaultAnswersDigitalLife>(entity =>
            {
                entity.ToTable("VaultAnswersDigitalLife");

                entity.Property(e => e.IdentifiantProfile).HasMaxLength(450);

                entity.Property(e => e.LegataireFirstLastName).HasMaxLength(450);

                entity.Property(e => e.OtherReseauSocial).HasMaxLength(450);

                entity.Property(e => e.ProfileUrl).HasMaxLength(2048);

                entity.Property(e => e.ProfileUrlLegataire).HasMaxLength(2048);

                entity.Property(e => e.ReseauSocial).HasMaxLength(10);

                entity.Property(e => e.VaultId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Vault)
                    .WithMany(p => p.VaultAnswersDigitalLives)
                    .HasForeignKey(d => d.VaultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VaultAnswersDigitalLife_Vaults");
            });

            modelBuilder.Entity<VaultAnswersHeritage>(entity =>
            {
                entity.ToTable("VaultAnswersHeritage");

                entity.Property(e => e.Comment).HasMaxLength(2048);

                entity.Property(e => e.Data).HasMaxLength(2048);

                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.VaultId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.JobNavigation)
                    .WithMany(p => p.VaultAnswersHeritages)
                    .HasForeignKey(d => d.Job)
                    .HasConstraintName("FK_VaultAnswersHeritage_JobParticular");

                entity.HasOne(d => d.Vault)
                    .WithMany(p => p.VaultAnswersHeritages)
                    .HasForeignKey(d => d.VaultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VaultAnswersHeritage_Vaults");
            });

            modelBuilder.Entity<VaultAnswersPersonal>(entity =>
            {
                entity.ToTable("VaultAnswersPersonal");

                entity.Property(e => e.Comment).HasMaxLength(2048);

                entity.Property(e => e.Data).HasMaxLength(2048);

                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.VaultId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.JobNavigation)
                    .WithMany(p => p.VaultAnswersPersonals)
                    .HasForeignKey(d => d.Job)
                    .HasConstraintName("FK_VaultAnswersPersonal_JobParticular");

                entity.HasOne(d => d.Vault)
                    .WithMany(p => p.VaultAnswersPersonals)
                    .HasForeignKey(d => d.VaultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VaultAnswersPersonale_Vaults");
            });

            modelBuilder.Entity<VaultAnticipationMeasuresInfo>(entity =>
            {
                entity.Property(e => e.DraftedBy).HasMaxLength(128);

                entity.Property(e => e.NecessaryTo).HasMaxLength(128);

                entity.Property(e => e.VaultId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Vault)
                    .WithMany(p => p.VaultAnticipationMeasuresInfos)
                    .HasForeignKey(d => d.VaultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VaultAnticipationMeasuresInfos_Vaults");
            });

            modelBuilder.Entity<VaultCategory>(entity =>
            {
                entity.ToTable("VaultCategory");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VaultContact>(entity =>
            {
                entity.Property(e => e.AdviceContractId).HasMaxLength(128);

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.VaultId).HasMaxLength(450);

                entity.HasOne(d => d.AccompanimentNavigation)
                    .WithMany(p => p.VaultContacts)
                    .HasForeignKey(d => d.Accompaniment)
                    .HasConstraintName("FK_VaultContacts_VaultCategory");

                entity.HasOne(d => d.Vault)
                    .WithMany(p => p.VaultContacts)
                    .HasForeignKey(d => d.VaultId)
                    .HasConstraintName("FK_VaultContacts_Vaults");
            });

            modelBuilder.Entity<VaultContract>(entity =>
            {
                entity.Property(e => e.ContractId)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.VaultId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Vault)
                    .WithMany(p => p.VaultContracts)
                    .HasForeignKey(d => d.VaultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VaultContracts_Vaults");
            });

            modelBuilder.Entity<VaultDigitalLife>(entity =>
            {
                entity.ToTable("VaultDigitalLife");

                entity.Property(e => e.IdentifiantProfile).HasMaxLength(450);

                entity.Property(e => e.LegataireFirstLastName).HasMaxLength(450);

                entity.Property(e => e.OtherReseauSocial).HasMaxLength(450);

                entity.Property(e => e.ProfileUrl).HasMaxLength(2048);

                entity.Property(e => e.ProfileUrlLegataire).HasMaxLength(2048);

                entity.Property(e => e.ReseauSocial).HasMaxLength(10);

                entity.Property(e => e.VaultId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Vault)
                    .WithMany(p => p.VaultDigitalLives)
                    .HasForeignKey(d => d.VaultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VaultDigitalLife_Vaults");
            });

            modelBuilder.Entity<VaultDocument>(entity =>
            {
                entity.Property(e => e.ContactId).HasMaxLength(450);

                entity.Property(e => e.ContractId).HasMaxLength(128);

                entity.Property(e => e.FileName).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(450);

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Url).HasMaxLength(450);

                entity.Property(e => e.VaultId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.VaultDocuments)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("FK_VaultDocuments_VaultContacts");

                entity.HasOne(d => d.Vault)
                    .WithMany(p => p.VaultDocuments)
                    .HasForeignKey(d => d.VaultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VaultDocuments_Vaults");
            });

            modelBuilder.Entity<VaultFamilyInfo>(entity =>
            {
                entity.Property(e => e.CoupleSituation).HasMaxLength(128);

                entity.Property(e => e.FamilialSituation).HasMaxLength(128);

                entity.Property(e => e.FamilyRelationships).HasMaxLength(128);

                entity.Property(e => e.MatrimonialSituation).HasMaxLength(128);

                entity.Property(e => e.VaultId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Vault)
                    .WithMany(p => p.VaultFamilyInfos)
                    .HasForeignKey(d => d.VaultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VaultFamilyInfos_Vaults");
            });

            modelBuilder.Entity<VaultFuneraryVolonte>(entity =>
            {
                entity.ToTable("VaultFuneraryVolonte");

                entity.Property(e => e.AshDestination).HasMaxLength(50);

                entity.Property(e => e.BurialChoice).HasMaxLength(50);

                entity.Property(e => e.BurialPlotNumber).HasMaxLength(128);

                entity.Property(e => e.Cimetery).HasMaxLength(128);

                entity.Property(e => e.City).HasMaxLength(128);

                entity.Property(e => e.FamilialBurialCity).HasMaxLength(128);

                entity.Property(e => e.FuneralConcessionaire).HasMaxLength(128);

                entity.Property(e => e.FuneralType).HasMaxLength(128);

                entity.Property(e => e.VaultId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Vault)
                    .WithMany(p => p.VaultFuneraryVolontes)
                    .HasForeignKey(d => d.VaultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VaultFuneraryVolonte_Vaults");
            });

            modelBuilder.Entity<VaultHeritage>(entity =>
            {
                entity.ToTable("VaultHeritage");

                entity.Property(e => e.BankAccountWording).HasMaxLength(450);

                entity.Property(e => e.BorrowingWording).HasMaxLength(450);

                entity.Property(e => e.CopyrightWording).HasMaxLength(450);

                entity.Property(e => e.IndustrialPropertyWording).HasMaxLength(450);

                entity.Property(e => e.LifeInsuranceWording).HasMaxLength(450);

                entity.Property(e => e.MainResidenceWording).HasMaxLength(450);

                entity.Property(e => e.MotorVehicleWording).HasMaxLength(450);

                entity.Property(e => e.OtherRealEstateWording).HasMaxLength(450);

                entity.Property(e => e.PortfolioOfSharesWording).HasMaxLength(450);

                entity.Property(e => e.SecondResidenceWording).HasMaxLength(450);

                entity.Property(e => e.UnitsSharesCompaniesWording).HasMaxLength(450);

                entity.Property(e => e.ValuablePersonalPropertyWording).HasMaxLength(450);

                entity.Property(e => e.VaultId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Vault)
                    .WithMany(p => p.VaultHeritages)
                    .HasForeignKey(d => d.VaultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VaultHeritage_Vaults");
            });

            modelBuilder.Entity<VaultPersonalInfo>(entity =>
            {
                entity.Property(e => e.Cv).HasColumnName("CV");

                entity.Property(e => e.HousingLaw).HasMaxLength(128);

                entity.Property(e => e.Job).HasMaxLength(128);

                entity.Property(e => e.LevelOfStudy).HasMaxLength(128);

                entity.Property(e => e.LivingEnvironment).HasMaxLength(128);

                entity.Property(e => e.OtherLanguage).HasMaxLength(128);

                entity.Property(e => e.PrecisionHousing).HasMaxLength(450);

                entity.Property(e => e.PrecisionLitigation).HasMaxLength(450);

                entity.Property(e => e.ProfessionalSituation).HasMaxLength(128);

                entity.Property(e => e.ProtectiveSupervision).HasMaxLength(128);

                entity.Property(e => e.SpeakFrench).HasMaxLength(128);

                entity.Property(e => e.TypeOfHousing).HasMaxLength(128);

                entity.Property(e => e.UnderstandFrench).HasMaxLength(128);

                entity.Property(e => e.VaultId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Vault)
                    .WithMany(p => p.VaultPersonalInfos)
                    .HasForeignKey(d => d.VaultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VaultPersonalInfos_Vaults");
            });

            modelBuilder.Entity<VaultTiersContact>(entity =>
            {
                entity.Property(e => e.AdviceContractId).HasMaxLength(128);

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.VaultId).HasMaxLength(450);

                entity.HasOne(d => d.AccompanimentNavigation)
                    .WithMany(p => p.VaultTiersContacts)
                    .HasForeignKey(d => d.Accompaniment)
                    .HasConstraintName("FK_VaultTiersContacts_VaultCategory");

                entity.HasOne(d => d.Vault)
                    .WithMany(p => p.VaultTiersContacts)
                    .HasForeignKey(d => d.VaultId)
                    .HasConstraintName("FK_VaultTiersContacts_Vaults");
            });

            modelBuilder.Entity<VaultUser>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.VaultId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.VaultUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VaultUsers_AspNetUsers");

                entity.HasOne(d => d.Vault)
                    .WithMany(p => p.VaultUsers)
                    .HasForeignKey(d => d.VaultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VaultUsers_Vaults");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
