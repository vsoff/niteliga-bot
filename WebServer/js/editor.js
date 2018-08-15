//function saveGame(id) {
//    try {
//        JSON.parse($('#game-editor-json')[0].value);
//    } catch {
//        alert('JSON неверного формата!');
//        return;
//    }
//    try {
//        JSON.parse($('#game-editor-setting')[0].value);
//    } catch {
//        alert('Setting неверного формата!');
//        return;
//    }
//    $.ajax({
//        type: 'POST',
//        url: '/api/Game/' + id + '/Update',
//        data: {
//            Caption: $('#game-editor-caption')[0].value,
//            JSON: $('#game-editor-json')[0].value,
//            Setting: $('#game-editor-setting')[0].value
//        }
//    }).done(res => {
//        console.log('done', res);
//        alert('Игра сохранена.');
//    }).fail(err => {
//        console.log('err', err);
//        alert('Ошибка при сохранении игры!');
//    });
//}

function getEditingId() {
    return (new URL(window.location.href)).searchParams.get("id");
}

let vm;

let viewModel = function (data) {
    $this = this;
    /// <=========================>
    /// <= Объявление переменных =>
    /// <=========================>
    $this.Id = ko.observable();
    $this.CreateDate = ko.observable();
    $this.Caption = ko.observable();
    // Переменные настроек
    $this.GameClosingDurationMin = ko.observable();
    $this.GameDurationMin = ko.observable();
    $this.Hint1DelaySec = ko.observable();
    $this.Hint2DelaySec = ko.observable();
    $this.TaskDropDelaySec = ko.observable();
    $this.SecondsDelayStart = ko.observable();
    $this.TeamIds = ko.observableArray();
    // Переменные конфигов
    $this.Config = ko.observable();
    // Редактор задания
    $this.Tasks = ko.observableArray();
    $this.Editor = {
        Task: ko.observable(),
        Hint1: ko.observable(),
        Hint2: ko.observable(),
        Address: ko.observable(),
        Code: ko.observable(),
        Lat: ko.observable(),
        Lon: ko.observable(),
        _this: ko.observable(null)
    };

    /// <======================>
    /// <= Объявление функций =>
    /// <======================>

    // Редактор заданий

    $this.editorSaveTask = function () {
        $this.copyTaskData($this.Editor._this(), $this.Editor);
        $this.Editor._this(null);
    }

    $this.editorCancel = function () {
        $this.Editor._this(null);
    }

    $this.editorPickTask = function () {
        $this.copyTaskData($this.Editor, this);
        $this.Editor._this(this);
    }

    $this.editorDeleteTask = function () {
        $this.Tasks.remove($this.Editor._this());
        $this.editorCancel();
    }

    $this.editorNewTask = function () {
        var newTask = {
            Task: ko.observable(''),
            Hint1: ko.observable(''),
            Hint2: ko.observable(''),
            Address: ko.observable(''),
            Code: ko.observable(''),
            Lat: ko.observable(null),
            Lon: ko.observable(null)
        };
        $this.Tasks.push(newTask);
    }

    $this.copyTaskData = function (target, val) {
        target.Task(val.Task());
        target.Hint1(val.Hint1());
        target.Hint2(val.Hint2());
        target.Address(val.Address());
        target.Code(val.Code());
        target.Lat(val.Lat());
        target.Lon(val.Lon());
    }

    // Другие



    $this.getVal = ((str) => {
        return str + "MYSTRING";
    });

    $this.loadData = ((data) => {
        $this.Id(data.Id);
        $this.CreateDate((new Date(data.CreateDate)).toLocaleString());
        $this.Caption(data.Caption);
        // Разбор настроек
        let settings = JSON.parse(data.Setting);
        $this.GameClosingDurationMin(settings.GameClosingDurationMin);
        $this.GameDurationMin(settings.GameDurationMin);
        $this.Hint1DelaySec(settings.Hint1DelaySec);
        $this.Hint2DelaySec(settings.Hint2DelaySec);
        $this.TaskDropDelaySec(settings.TaskDropDelaySec);
        $this.SecondsDelayStart(settings.SecondsDelayStart);
        $this.TeamIds(settings.TeamIds);
        // Разбор конфига
        let configs = JSON.parse(data.Config);
        console.log(configs)

        for (var i in configs.Tasks) {
            var curTask = {
                Task: ko.observable(configs.Tasks[i].Task),
                Hint1: ko.observable(configs.Tasks[i].Hint1),
                Hint2: ko.observable(configs.Tasks[i].Hint2),
                Address: ko.observable(configs.Tasks[i].Address),
                Code: ko.observable(configs.Tasks[i].Code),
                Lat: ko.observable(configs.Tasks[i].Lat),
                Lon: ko.observable(configs.Tasks[i].Lon)
            };
            $this.Tasks.push(curTask);
        }


        $this.Config(data.Config);
    });
}



// Запуск всего
window.onload = () => {
    vm = new viewModel(null);
    ko.applyBindings(vm);

    $.ajax({
        type: 'GET',
        url: '/api/game/' + getEditingId()
    }).done(res => {
        console.log('Getted data', res.data);
        // Load & mount data
        vm.loadData(res.data);
        //// Show schedule element
        //vm.setLoadStatus(true);
        //$('#schedule-div').show();
    }).fail(res => {
        console.log('fail', res);
    });
}