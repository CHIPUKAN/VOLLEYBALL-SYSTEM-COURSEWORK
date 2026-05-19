namespace VolleyballIS.Domain.Entities
{
    // Т16h. Журнал изменений протокола
    public class T16ProtocolHistory
    {
        #region Поля
        private int id; // первичный ключ
        #endregion

        #region Свойства
        public int Id // идентификатор записи журнала
        {
            get => id;
            set => id = value;
        }

        public int ProtocolId { get; set; } // идентификатор протокола (внешний ключ)

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow; // дата и время изменения

        public short? StatusAtMoment { get; set; } // статус протокола на момент изменения

        public string? DataHash { get; set; } // хеш данных протокола для аудита (необязательно)

        public string? Comment { get; set; } // комментарий к изменению (необязательно)

        // навигационное свойство
        public T16Protocol? Protocol { get; set; } // протокол
        #endregion

        #region Конструкторы
        public T16ProtocolHistory() // конструктор по умолчанию (нужен EF Core)
        {
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"[{Id}] Изменение протокола {ProtocolId} в {ChangedAt:dd.MM.yyyy HH:mm}";
        }
        #endregion
    }
}
