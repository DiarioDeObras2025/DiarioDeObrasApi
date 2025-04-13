using System.ComponentModel.DataAnnotations;

namespace DiarioObras.DTOs.ObraDTOs;

public class ObraDTONome : IValidatableObject
{

    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Nome is null)
            yield return new ValidationResult("o nome não pode ser vazio", new[] {nameof(this.Nome)});
    }
}
