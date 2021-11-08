function handleClick(e) {
    let element = e.target;

    let row = Number(element.getAttribute('data-row'))
    let col = Number(element.getAttribute('data-col'))

    let gameId = Number(window.location.href.split('/').pop())

    let shot = {}
    shot.Col = col;
    shot.Row = row;
    shot.GameId = gameId

    $.ajax({
        type: "POST",
        url: '/Game/MakeShot',
        data: JSON.stringify({
            'Col': col - 1,
            'Row': row - 1,
            'GameId': gameId
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        error: function (err) {
            console.log(err);
        }
    })
        .done(function (data) {
            drawCell(data, element)
        });
}

function getNodeByColAndRow(point) {
    let elements = document.querySelectorAll(`div[data-field="${point.owner}"]`);
    return [...elements].filter(item => item.getAttribute('data-col') == point.col && item.getAttribute('data-row') == point.row)[0];
}

function drawCell(data, target) {
    if (data.winner) {
        Swal.fire({
            icon: 'success',
            title: data.winner + " won!!!<br /> You will be redirected to profile page",
            sconfirmButtonColor: '#3085d6',
            confirmButtonText: 'Ok'
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = '/Home/Profile'
            }
        })  
    }

    if (data.userShotCellType === "HIT") {
        target.style.background = '#f72323';
    } else if (data.userShotCellType === 'MISS') {
        target.style.background = '#a2d5fc'
    }

    target.disabled = true;

    let node = getNodeByColAndRow({ owner: "user", col: data.col + 1, row: data.row + 1 })
    if (data.computerShotCellType === 'HIT') {
        node.style.background = '#f72323';
    } else if (data.computerShotCellType === 'MISS') {
        node.style.background = '#a2d5fc'
    }
}