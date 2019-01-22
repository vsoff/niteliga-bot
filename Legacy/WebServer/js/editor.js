let vm;

// Возвращает из URL query параметр `id`
function getEditingId() {
    return (new URL(window.location.href)).searchParams.get("id");
}

// Генерирует случайный цвет
let getRandomColor = (() => {
    let letters = '0123456789ABCDEF';
    let color = '#';
    for (let i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
});

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
    // Переменные сетки
    //$this.GridTaskCountArr = ko.observableArray([]);
    //$this.GridWayCountArr = ko.observableArray([]);
    $this.Grid = ko.observableArray([]);
    $this.gridCellValue = ko.observable(-1);
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
        $this.Tasks.push({
            Task: ko.observable(''),
            Hint1: ko.observable(''),
            Hint2: ko.observable(''),
            Address: ko.observable(''),
            Code: ko.observable(''),
            Lat: ko.observable(0.0/*null*/),
            Lon: ko.observable(0.0/*null*/),
            _color: ko.observable(getRandomColor())
        });
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

    // Редактор сеток

    $this.setGridPattern = (($data) => {
        $this.gridCellValue($data);
    });

    $this.setGridCell = ((col, row, $data) => {
        console.log('[GRID CLICK] data: ' + $data + '=>' + $this.gridCellValue(), '| col: ' + col, '| row: ' + row);
        $this.Grid()[row]()[col]($this.gridCellValue());
    });

    $this.getTaskTask = (($data) => {
        return $this.Tasks()[$data] ? $this.Tasks()[$data].Task() : null;
    });

    $this.getTaskColor = (($data) => {
        return $this.Tasks()[$data] ? $this.Tasks()[$data]._color : '#FFFFFF';
    });

    // Увеличивает кол-во сеток
    $this.increaseWayCount = (() => {
        let arr = ko.observableArray();
        let tasksCount = $this.Grid().length > 0 ? $this.Grid()[0]().length : 0;
        for (let j = 0; j < tasksCount; j++) {
            arr.push(ko.observable(-1));
        }
        $this.Grid.push(arr);
    });

    // Увеличивает кол-во заданий в сетке
    $this.increaseTaskCount = (() => {
        for (let j = 0; j < $this.Grid().length; j++)
            $this.Grid()[j].push(ko.observable(-1));
    });

    // Уменьшает кол-во сеток
    $this.reduceWayCount = (() => {
        $this.Grid.pop();
    });

    // Уменьшает кол-во заданий в сетке
    $this.reduceTaskCount = (() => {
        for (let j = 0; j < $this.Grid().length; j++)
            $this.Grid()[j].pop(ko.observable(-1));
    });

    // Другие функции

    // Преобразует данные для редактора
    $this.loadData = ((data) => {
        $this.Id(data.Id);
        $this.CreateDate((new Date(data.CreateDate)).toLocaleString());
        $this.Caption(data.Caption);
        // Разбор настроек
        let settings = JSON.parse(data.Setting);
        console.log('Setting', settings)

        $this.GameClosingDurationMin(settings.GameClosingDurationMin);
        $this.GameDurationMin(settings.GameDurationMin);
        $this.Hint1DelaySec(settings.Hint1DelaySec);
        $this.Hint2DelaySec(settings.Hint2DelaySec);
        $this.TaskDropDelaySec(settings.TaskDropDelaySec);
        $this.SecondsDelayStart(settings.SecondsDelayStart);
        $this.TeamIds(settings.TeamIds.join(','));
        // Разбор конфига
        let configs = JSON.parse(data.Config);
        console.log('Config', configs)

        for (let i in configs.TaskGrid) {
            let arr = ko.observableArray();
            for (let j = 0; j < (configs.TaskGrid.length > 0 ? configs.TaskGrid[0].length : 0); j++) {
                arr.push(ko.observable(configs.TaskGrid[i][j]));
            }
            $this.Grid.push(arr);
        }

        for (var i in configs.Tasks) {
            $this.Tasks.push({
                Task: ko.observable(configs.Tasks[i].Task),
                Hint1: ko.observable(configs.Tasks[i].Hint1),
                Hint2: ko.observable(configs.Tasks[i].Hint2),
                Address: ko.observable(configs.Tasks[i].Address),
                Code: ko.observable(configs.Tasks[i].Code),
                Lat: ko.observable(configs.Tasks[i].Lat),
                Lon: ko.observable(configs.Tasks[i].Lon),
                _color: ko.observable(getRandomColor())
            });
        }
    });

    // Формирует объект для сохранения и отправляет его на сервер
    $this.saveData = (() => {
        let data = {};
        data.Caption = $this.Caption();

        // Собираем список id команд
        let teamIds = [];
        console.log($this.TeamIds())
        $this.TeamIds().split(',').forEach(e => {
            if (parseInt(e) > 0) teamIds.push(parseInt(e))
        });

        // Записываем все настройки
        data.Setting = JSON.stringify({
            GameClosingDurationMin: $this.GameClosingDurationMin(),
            GameDurationMin: $this.GameDurationMin(),
            Hint1DelaySec: $this.Hint1DelaySec(),
            Hint2DelaySec: $this.Hint2DelaySec(),
            TaskDropDelaySec: $this.TaskDropDelaySec(),
            SecondsDelayStart: $this.SecondsDelayStart(),
            TeamIds: teamIds
        });

        let config = {
            Tasks: [],
            TaskGrid: []
        };
        $this.Tasks().forEach(e => {
            config.Tasks.push({
                Task: e.Task(),
                Hint1: e.Hint1(),
                Hint2: e.Hint2(),
                Address: e.Address(),
                Code: e.Code(),
                Lat: e.Lat(),
                Lon: e.Lon(),
            });
        });

        let grid = [];
        $this.Grid().forEach(arr => {
            let way = [];
            arr().forEach(e => {
                way.push(e());
            });
            config.TaskGrid.push(way);
        });

        data.Config = JSON.stringify(config);

        console.log('Object to save', data);

        // Отправка на сервер
        $.ajax({
            type: 'POST',
            url: '/api/Game/' + getEditingId() + '/Update',
            data: data
        }).done(res => {
            console.log('done', res);
            alert('Игра сохранена.');
        }).fail(err => {
            console.log('err', err);
            alert('Ошибка при сохранении игры!');
        });
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
    }).fail(res => {
        console.log('fail', res);
    });
}
