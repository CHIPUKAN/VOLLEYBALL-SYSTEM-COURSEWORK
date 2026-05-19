namespace VolleyballIS.Domain.Entities
{
    // Т7. Заявка команды на турнир
    public class T7Application
    {
        #region Поля
        private int id; // первичный ключ
        #endregion

        #region Свойства
        public int Id // идентификатор заявки
        {
            get => id;
            set => id = value;
        }

        public int TeamId { get; set; } // идентификатор команды (внешний ключ)

        public int TournamentId { get; set; } // идентификатор турнира (внешний ключ)

        public DateOnly SubmissionDate { get; set; } // дата подачи заявки

        public short StatusCode { get; set; } = 1; // код статуса заявки (внешний ключ -> s3_application_statuses)

        // навигационные свойства
        public T4Team? Team { get; set; }                            // команда-заявитель
        public T10Tournament? Tournament { get; set; }               // турнир
        public S3ApplicationStatus? Status { get; set; }             // статус заявки
        public ICollection<T8ApplicationComposition> Composition { get; set; } = new List<T8ApplicationComposition>(); // состав заявки
        #endregion

        #region Конструкторы
        public T7Application() // конструктор по умолчанию (нужен EF Core)
        {
        }

        public T7Application(int teamId, int tournamentId) // конструктор с параметрами
        {
            TeamId = teamId;
            TournamentId = tournamentId;
            SubmissionDate = DateOnly.FromDateTime(DateTime.Today);
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"[{Id}] Заявка команды {TeamId} на турнир {TournamentId}";
        }
        #endregion
    }
}
