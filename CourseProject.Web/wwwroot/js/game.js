class Ship {
    constructor(size, start = null, end  = null, isPalced = false) {
        this.size = size;
        this.start = start;
        this.end = end;
        this.isPalced = isPalced;
    }
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

document.getElementById('palce_button').addEventListener('click', function(e) {
    e.preventDefault();
    size = document.getElementById('size').value;
    start_row = document.getElementById('start-row').value;
    start_col = characters.indexOf(document.getElementById('start-col').value) + 1;

    end_row = document.getElementById('end-row').value;
    end_col = characters.indexOf(document.getElementById('end-col').value) + 1;

    let ship = getUnplacedShip(ships);
    if (ship == null) {
        let elem = document.getElementById('ship_placement');
        elem.parentNode.removeChild(elem);
        document.getElementById('start_game').removeAttribute("disabled");
        return;
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



