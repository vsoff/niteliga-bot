function launchGame() {
    console.log('launching game ' + id + '...');
    var id = $('#launchPanelIdSelect')[0].value;

    $.ajax({
        type: 'POST',
        url: '/api/Game/' + id + '/Start'
    }).done(res => {
        console.log('done', res);
        if (res.error)
            alert('При запуске игры произошла ошибка: `' + res.error + '`');
        else
            alert('Игра запущена!')
    }).fail(err => {
        console.log('err', err);
    });
}

