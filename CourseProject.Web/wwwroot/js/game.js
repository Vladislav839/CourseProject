class Ship {
    constructor(size, start = null, end  = null, isPalced = false) {
        this.size = size;
        this.start = start;
        this.end = end;
        this.isPalced = isPalced;
    }
}

let field = new Array(10);

for (let j = 0; j < 10; j++) {
    field[j] = new Array(10).fill(0);
}

const characters = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J']

let ships = []

ships.push(new Ship(4));
ships.push(new Ship(3));
ships.push(new Ship(3));
ships.push(new Ship(2));
ships.push(new Ship(2));
ships.push(new Ship(2));
ships.push(new Ship(1));
ships.push(new Ship(1));
ships.push(new Ship(1));
ships.push(new Ship(1));

let count = 0;

const getUnplacedShip = (ships) => {
    for (let item of ships) {
        if (item.isPalced == false) {
            return item
        }
    }
    return null;
}

function getNodeByColAndRow(point) {
    let elements = document.querySelectorAll(`div[data-row="${point.row}"]`);
    return [...elements].filter(item => item.getAttribute('data-col') == point.col)[0];
}

function drawShiponBoard(startNode, endNode) {
    console.log(startNode);
    console.log(endNode);
    startNode.style.background = 'grey';
    endNode.style.background = 'grey';
    if (startNode.getAttribute('data-row') == endNode.getAttribute('data-row')) {
        for (let i = Number(startNode.getAttribute('data-col')) + 1; i < Number(endNode.getAttribute('data-col')); i++) {
            getNodeByColAndRow({
                row: startNode.getAttribute('data-row'),
                col: i.toString()
            }).style.background = 'grey'
        }
    } else if (startNode.getAttribute('data-col') == endNode.getAttribute('data-col')) {
        for (let i = Number(startNode.getAttribute('data-row')) + 1; i < Number(endNode.getAttribute('data-row')); i++) {
            getNodeByColAndRow({
                row: i.toString(),
                col: startNode.getAttribute('data-col')
            }).style.background = 'grey'
        }
    }
}

function placeShip(ship) {
    let startElement = getNodeByColAndRow(ship.start);

    let endElement = getNodeByColAndRow(ship.end);

    drawShiponBoard(startElement, endElement);
}

function drawNext(c) {
    if (c == 10) {
        let elem = document.getElementById('ship_placement');
        elem.parentNode.removeChild(elem);
        document.getElementById('start_game').removeAttribute("disabled");
        return;
    }
    let size = ships[c].size;
    document.getElementById('size').value = size;
    document.getElementById('ship_preview').innerHTML = '<div class="ship" style="width: ' + size * 30 + 'px;"></div>'
    if (size == 1) {
        end = document.getElementById('end').style.display = 'none';
    }
}

function checkRules(startRow, startCol, dir, size) {
    let fromX, toX, fromY, toY

    fromX = (startRow == 0) ? startRow : startRow - 1;

    if (startRow + size == 10 && dir == 1) toX = startRow + size;
    else if (startRow + size < 10 && dir == 1) toX = startRow + size + 1;
    else if (startRow == 9 && dir == 0) toX = startRow + 1;
    else if (startRow < 9 && dir == 0) toX = startRow + 2;

    fromY = (startCol == 0) ? startCol : startCol - 1;
    if (startCol + size == 10 && dir == 1) toY = startCol + size;
    else if (startCol + size < 10 && dir == 1) toY = startCol + size + 1;
    else if (startCol == 9 && dir == 0) toY = startCol + 1;
    else if (startCol < 9 && dir == 0) toY = startCol + 2;

    if (toX == undefined || toY == undefined) return false;

    for (let i = fromX; i < toX; i++)
    {
        for (let j = fromY; j < toY; j++)
        {
            if (field[i][j] == 1) return false;
        }
    }

    return true;


}

document.getElementById('palce_button').addEventListener('click', function(e) {
    e.preventDefault();
    size = document.getElementById('size').value;
    start_row = Number(document.getElementById('start-row').value);
    start_col = characters.indexOf(document.getElementById('start-col').value) + 1;

    end_row = Number(document.getElementById('end-row').value);
    end_col = characters.indexOf(document.getElementById('end-col').value) + 1;

    if (start_row == end_row) {
        if (Math.abs(Number(start_col) - Number(end_col)) + 1 != Number(size)) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Неверные координаты',
            })
            return
        }
    }
    else if (start_col == end_col) {
        if (Math.abs(Number(start_row) - Number(end_row)) + 1 != Number(size)) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Неверные координаты',
            })
            return
        }
    }
    else {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Неверные координаты',
        })
        return
    }

    let ship = getUnplacedShip(ships);
    if (ship == null) {
        let elem = document.getElementById('ship_placement');
        elem.parentNode.removeChild(elem);
        document.getElementById('start_game').removeAttribute("disabled");
        return;
    }

    let dir

    if (Number(start_row) == Number(end_row)) dir = 0
    if (Number(start_col) == Number(end_col)) dir = 1

    let res = checkRules(Number(start_row) - 1, Number(start_col) - 1, dir, Number(size))

    if (!res) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Там уже есть корабль',
        })
        return;
    }

    if (dir == 0) {
        for (let i = Number(start_col) - 1; i < Number(start_col) - 1 + Number(size); i++) {
            field[Number(start_row) - 1][i] = 1;
        }
    } else {
        for (let i = Number(start_row) - 1; i < Number(start_row) - 1 + Number(size); i++) {
            field[i][Number(start_col) - 1] = 1;
        }
    }

    ship.start = { row: start_row, col: start_col };
    ship.end = { row: end_row, col: end_col };
    ship.size = size;
    ship.isPalced = true;

    if (ship.size == 1) {
        getNodeByColAndRow(ship.start).style.background = 'grey';
    } else {
        placeShip(ship);
    }

    drawNext(++count);
})

document.getElementById('start_game').addEventListener('click', function (e) {

    let nodes = document.querySelector('.wrapper').children;
    for (let i = 0; i < nodes.length; i++) {
        if (nodes[i].getAttribute('data-col') == "0" || nodes[i].getAttribute('data-row') == "0") {
            continue;
        }

        if (nodes[i].style.background == 'grey') {
            field[Number(nodes[i].getAttribute('data-row')) - 1][Number(nodes[i].getAttribute('data-col')) - 1] = 1;
        } else {
            field[Number(nodes[i].getAttribute('data-row')) - 1][Number(nodes[i].getAttribute('data-col')) - 1] = 0;
        }
    }

    $.ajax({
        url: '/Game/InititalizeGame',
        data: JSON.stringify(field),
        type: "POST",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

        },
        error: function (data, textStatus) { }
    }).done(function (id) {
        window.location.href = "Game/GameSession/" + id;
        });

})


