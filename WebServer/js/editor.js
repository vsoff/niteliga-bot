function saveGame(id) {
    try {
        JSON.parse($('#game-editor-json')[0].value);
    } catch {
        alert('JSON неверного формата!');
        return;
    }

    try {
        JSON.parse($('#game-editor-setting')[0].value);
    } catch {
        alert('Setting неверного формата!');
        return;
    }

    $.ajax({
        type: 'POST',
        url: '/api/Game/' + id + '/Update',
        data: {
            Caption: $('#game-editor-caption')[0].value,
            JSON: $('#game-editor-json')[0].value,
            Setting: $('#game-editor-setting')[0].value
        }
    }).done(res => {
        console.log('done', res);
        alert('Игра сохранена.');
    }).fail(err => {
        console.log('err', err);
        alert('Ошибка при сохранении игры!');
    });
}
