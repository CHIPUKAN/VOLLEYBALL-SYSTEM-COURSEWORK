namespace VolleyballIS.Application.DTOs
{
    // Универсальный DTO для справочника с числовым кодом (используется во всех С1–С18)
    public class LookupDto
    {
        #region Свойства
        public short Code { get; set; } // код записи справочника

        public string Name { get; set; } = string.Empty; // наименование
        #endregion
    }

    // Универсальный DTO для справочника с целочисленным id (регионы, тренеры, площадки и т.д.)
    public class LookupItemDto
    {
        #region Свойства
        public string Id { get; set; } = string.Empty; // идентификатор (string — принимает и int, и ОКТМО)

        public string Name { get; set; } = string.Empty; // отображаемое наименование
        #endregion
    }
}
