const sendRunePageBtns = document.querySelectorAll('.send-rune-page');

sendRunePageBtns.forEach(btn => {
    const id = btn.dataset.id;

    btn.addEventListener('click',
        (e) => {
            e.preventDefault();

            fetch(`/lol/api/putrunepage?id=${id}`, { method: 'POST' })
                .catch(error => console.log('error', error));
        });
});