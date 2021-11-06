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

function getPoint(info) {
    let point = {}
    point.row = info.substring(1, 2);
    point.col = characters.indexOf(info.substring(0, 1)) + 1;
    return point;
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
    let start = getPoint(ship.start);
    let end = getPoint(ship.end);

    let startElement = getNodeByColAndRow(start);

    let endElement = getNodeByColAndRow(end);

    drawShiponBoard(startElement, endElement);
}

function drawNext(c) {
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
    start = document.getElementById('start').value;
    end = document.getElementById('end').value;

    let ship = getUnplacedShip(ships);
    if (ship == null) {
        document.getElementById('ship_placement').style.display = 'none';
        document.getElementById('start_game').setAttribute('disabled') = false;
        return;
    }
    ship.start = start;
    ship.end = end;
    ship.size = size;
    ship.isPalced = true;

    placeShip(ship);

    document.getElementById('start').value = '';
    document.getElementById('end').value = '';

    drawNext(++count);
})



