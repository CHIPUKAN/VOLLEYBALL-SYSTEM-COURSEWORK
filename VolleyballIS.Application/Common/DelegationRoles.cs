using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.Common
{
    // Допустимые роли членов делегации команды на матч
    public static class DelegationRoles
    {
        public const string AssistantCoach = "помощник тренера";
        public const string Masseur        = "массажист";
        public const string Doctor         = "врач";

        public static readonly string[] All = [AssistantCoach, Masseur, Doctor];
    }

    // Атрибут валидации роли делегации
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DelegationRoleValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            if (value is string s && DelegationRoles.All.Contains(s))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(
                $"Недопустимая роль. Разрешены: {string.Join(", ", DelegationRoles.All)}");
        }
    }
}
