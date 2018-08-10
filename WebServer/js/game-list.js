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
