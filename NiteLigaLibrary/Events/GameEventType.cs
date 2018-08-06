namespace NiteLigaLibrary.Events
{
    /// <summary>
    /// Перечисление типов событий в игре NiteLiga
    /// </summary>
    public enum GameEventType
    {
        /// <summary>
        /// Игра началась.
        /// </summary>
        GameStarted,
        /// <summary>
        /// Игра прервана.
        /// </summary>
        GameAborted,
        /// <summary>
        /// Игра закончена.
        /// </summary>
        GameStopped,
        /// <summary>
        /// Игра восстановлена (после перезапуска сервера).
        /// </summary>
        GameRestored,
        /// <summary>
        /// Игра сохранена.
        /// </summary>
        GameSaved,

        /// <summary>
        /// Игрок сдаёт задание правильно.
        /// </summary>
        PlayerCompletedTask,
        /// <summary>
        /// Игрок сдаёт задание неправильно.
        /// </summary>
        PlayerFailedTask,

        /// <summary>
        /// Команда начинает играть.
        /// </summary>
        TeamStartsPlay,
        /// <summary>
        /// Команда получает подсказку (слив).
        /// </summary>
        TeamGetHint,
        /// <summary>
        /// Команда получает адрес (слив).
        /// </summary>
        TeamGetAddress,
        /// <summary>
        /// Команда сливает задание.
        /// </summary>
        TeamDropTask,

        /// <summary>
        /// Система или организатор отправил широковещательное сообщение.
        /// </summary>
        AdminSendMessage,
    }
}
