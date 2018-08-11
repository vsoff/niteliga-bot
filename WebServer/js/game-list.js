function deleteGame(id) {
    $.ajax({
        type: 'DELETE',
        url: '/api/Game/' + id
    }).done(res => {
        console.log('done', res);
        window.location.reload();
    }).fail(err => {
        console.log('err', err);
    });
}

function addGame() {
    $.ajax({
        type: 'POST',
        url: '/api/Game'
    }).done(res => {
        console.log('done', res);
        window.location.reload();
    }).fail(err => {
        console.log('err', err);
    });
}

function verifyGame(id) {
    $.ajax({
        type: 'POST',
        url: '/api/Game/' + id + '/Verify'
    }).done(res => {
        console.log('done', res);
        if (!res.errorList)
            alert('Игра полностью соответствует шаблону!');
        else
            alert('Ошибка: ' + res.error + '\nПеречень ошибок: ' + JSON.stringify(res.errorList))
    }).fail(err => {
        console.log('err', err);
    });
}

