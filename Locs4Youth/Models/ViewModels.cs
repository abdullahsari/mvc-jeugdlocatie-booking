using Locs4Youth.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Locs4Youth.Models
{
    #region Rental
    public class RentModel : IValidatableObject
    {
        [Required]
        public int LocationID { get; set; }

        [Display(Name = "Bericht (optioneel)")]
        public string Message { get; set; }

        [Display(Name = "Van")]
        [Required(ErrorMessage = "Gelieve een begindatum te kiezen.")]
        [DataType(DataType.Date, ErrorMessage = "Dit veld moet een datum bevatten.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [ValidateDate]
        public DateTime From { get; set; }

        [Display(Name = "Tot")]
        [Required(ErrorMessage = "Gelieve een einddatum te kiezen.")]
        [DataType(DataType.Date, ErrorMessage = "Dit veld moet een datum bevatten.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [ValidateDate]
        public DateTime To { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (From > To)
            {
                yield return new ValidationResult(errorMessage: "Einddatum moet groter of gelijk zijn aan de startdatum.", memberNames: new[] { "To" });
            }
        }
    }
    #endregion

    #region Locations
    public enum Region
    {
        Antwerpen,
        Brussel,
        Limburg,
        [Display(Name = "Oost-Vlaanderen")]
        OostVlaanderen,
        [Display(Name = "Vlaams-Brabant")]
        VlaamsBrabant,
        [Display(Name = "West-Vlaanderen")]
        WestVlaanderen
    }

    public class OfferModel
    {
        [Display(Name = "Naam")]
        [Required(ErrorMessage = "Gelieve een naam voor de locatie in te vullen.")]
        [MaxLength(50, ErrorMessage = "De naam kan maximaal 50 karakters bevatten.")]
        public string Title { get; set; }

        [Display(Name = "Beschrijving")]
        [Required(ErrorMessage = "Gelieve een beschrijving voor de locatie toe te voegen.")]
        [MinLength(20, ErrorMessage = "De beschrijving moet minimaal 20 karakters bevatten.")]
        [MaxLength(5000, ErrorMessage = "De beschrijving kan maximaal 5000 karakters bevatten.")]
        [AllowHtml]
        public string Description { get; set; }

        [Display(Name = "Adres")]
        [Required(ErrorMessage = "Gelieve het adres in te vullen.")]
        [MaxLength(256, ErrorMessage = "Het adres kan maximaal 256 karakters bevatten.")]
        public string Address { get; set; }

        [Display(Name = "Plaatsnaam")]
        [Required(ErrorMessage = "Gelieve de plaatsnaam in te vullen.")]
        [MaxLength(50, ErrorMessage = "De plaatsnaam kan maximaal 50 karakters bevatten.")]
        public string City { get; set; }

        [Display(Name = "Maximum Aantal Personen")]
        [Required(ErrorMessage = "Gelieve de capaciteit van de locatie in te vullen.")]
        [Range(1, int.MaxValue, ErrorMessage = "De capaciteit begint vanaf 1 persoon.")]
        public int Capacity { get; set; }

        [Display(Name = "Regio")]
        [Required(ErrorMessage = "Gelieve een regio te kiezen.")]
        public Region? Region { get; set; }

        [Display(Name = "Faciliteiten")]
        public List<CheckBoxListItem> Features { get; set; }

        [Display(Name = "Afbeelding uploaden")]
        [Required(ErrorMessage = "Gelieve een afbeelding up te loaden met een degelijke kwaliteit.")]
        [ValidateFile]
        public HttpPostedFileBase Image { get; set; }

        public OfferModel()
        {
            Features = new List<CheckBoxListItem>();
        }
    }

    public class EditModel
    {
        [Display(Name = "Naam")]
        [Required(ErrorMessage = "Gelieve een naam voor de locatie in te vullen.")]
        [MaxLength(50, ErrorMessage = "De naam kan maximaal 50 karakters bevatten.")]
        public string Title { get; set; }

        [Required]
        public int LocationID { get; set; }

        [Display(Name = "Beschrijving")]
        [Required(ErrorMessage = "Gelieve een beschrijving voor de locatie toe te voegen.")]
        [MinLength(20, ErrorMessage = "De beschrijving moet minimaal 20 karakters bevatten.")]
        [MaxLength(5000, ErrorMessage = "De beschrijving kan maximaal 5000 karakters bevatten.")]
        [AllowHtml]
        public string Description { get; set; }

        [Display(Name = "Adres")]
        [Required(ErrorMessage = "Gelieve het adres in te vullen.")]
        [MaxLength(256, ErrorMessage = "Het adres kan maximaal 256 karakters bevatten.")]
        public string Address { get; set; }

        [Display(Name = "Plaatsnaam")]
        [Required(ErrorMessage = "Gelieve de plaatsnaam in te vullen.")]
        [MaxLength(50, ErrorMessage = "De plaatsnaam kan maximaal 50 karakters bevatten.")]
        public string City { get; set; }

        [Display(Name = "Maximum Aantal Personen")]
        [Required(ErrorMessage = "Gelieve de capaciteit van de locatie in te vullen.")]
        [Range(1, int.MaxValue, ErrorMessage = "De capaciteit begint vanaf 1 persoon.")]
        public int Capacity { get; set; }

        [Display(Name = "Regio")]
        [Required(ErrorMessage = "Gelieve een regio te kiezen.")]
        public Region? Region { get; set; }

        [Display(Name = "Faciliteiten")]
        public List<CheckBoxListItem> Features { get; set; }

        [Display(Name = "Afbeelding uploaden")]
        [ValidateFile]
        public HttpPostedFileBase Image { get; set; }

        public EditModel()
        {
            Features = new List<CheckBoxListItem>();
        }
    }

    public class RateModel
    {
        [Required]
        public int LocationID { get; set; }

        [Required(ErrorMessage = "Gelieve een bericht in te geven.")]
        [Display(Name = "Beoordeling")]        
        public string Message { get; set; }

        [Required(ErrorMessage = "Gelieve een score mee te geven.")]
        [Range(1, 5, ErrorMessage = "De score moet tussen 1 en 5 zijn.")]
        [Display(Name = " ")]
        public int Score { get; set; }

        public RateModel()
        {
            Score = 3;
        }
    }

    public class FilterModel : IValidatableObject
    {
        [Display(Name = "Locatie")]
        [MaxLength(50, ErrorMessage = "De naam kan maximaal 50 karakters bevatten.")]
        public string Title { get; set; }

        [Display(Name = "Van")]
        [DataType(DataType.Date, ErrorMessage = "Dit veld moet een datum bevatten.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [ValidateDate]
        public DateTime? From { get; set; }

        [Display(Name = "Tot")]
        [DataType(DataType.Date, ErrorMessage = "Dit veld moet een datum bevatten.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [ValidateDate]
        public DateTime? To { get; set; }

        [Display(Name = "Personen")]
        [Range(1, int.MaxValue, ErrorMessage = "De capaciteit begint vanaf 1 persoon.")]
        public int? Capacity { get; set; }

        [Display(Name = "Regio")]
        public Region? Region { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (From.HasValue && To.HasValue)
            {
                if (From.Value > To.Value)
                {
                    yield return new ValidationResult(errorMessage: "Einddatum moet groter of gelijk zijn aan de startdatum.", memberNames: new[] { "To" });
                }
            }
        }
    }
    #endregion

    #region Account
    public class AvatarModel
    {
        [Required(ErrorMessage = "Om uw avatar te wijzigen moet u een afbeelding uploaden.")]
        [Display(Name = "Uploaden")]
        [ValidateFile]
        public HttpPostedFileBase Image { get; set; }
    }

    public class PasswordModel
    {
        [Display(Name = "Huidig wachtwoord")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Gelieve uw huidig wachtwoord in te vullen.")]
        public string OldPassword { get; set; }

        [Display(Name = "Nieuw wachtwoord")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Gelieve uw nieuw wachtwoord in te vullen.")]
        public string NewPassword { get; set; }

        [Display(Name = "Bevestigen")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Gelieve uw nieuw wachtwoord te bevestigen.")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "De wachtwoorden komen niet overeen.")]
        public string ConfirmPassword { get; set; }
    }

    public class ProfileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class SignInModel
    {
        [Display(Name = "E-mailadres")]
        [Required(ErrorMessage = "Gelieve uw e-mailadres in te vullen.")]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres.")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Gelieve uw wachtwoord in te vullen.")]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [Display(Name = "Ingelogd blijven")]
        public bool RememberMe { get; set; }
    }
    #endregion
}