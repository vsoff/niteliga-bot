namespace NL.NiteLiga.Core.Events
{
    /// <summary>
    /// Перечисление типов событий в игре NiteLiga
    /// </summary>
    public enum GameEventType
    {
        /// <summary>
        /// Игра началась.
        /// </summary>
        GameStarted = 0,
        /// <summary>
        /// Игра прервана.
        /// </summary>
        GameAborted = 1,
        /// <summary>
        /// Игра закончена.
        /// </summary>
        GameStopped = 2,
        /// <summary>
        /// Игра восстановлена (после перезапуска сервера).
        /// </summary>
        GameRestored = 3,
        /// <summary>
        /// Игра сохранена.
        /// </summary>
        GameSaved = 4,

        /// <summary>
        /// Игрок сдаёт задание правильно.
        /// </summary>
        PlayerCompletedTask = 5,
        /// <summary>
        /// Игрок сдаёт задание неправильно.
        /// </summary>
        PlayerFailedTask = 6,

        /// <summary>
        /// Команда начинает играть.
        /// </summary>
        TeamStartsPlay = 7,
        /// <summary>
        /// Команда получает подсказку (слив).
        /// </summary>
        TeamGetHint = 8,
        /// <summary>
        /// Команда получает адрес (слив).
        /// </summary>
        TeamGetAddress = 9,
        /// <summary>
        /// Команда сливает задание.
        /// </summary>
        TeamDropTask = 10,

        /// <summary>
        /// Система или организатор отправил широковещательное сообщение.
        /// </summary>
        AdminSendMessage = 11,
    }
}
