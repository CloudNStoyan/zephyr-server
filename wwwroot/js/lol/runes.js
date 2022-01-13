(async () => {

    const runesReforged = await fetch('/js/lol/runesReforged.json').then(resp => resp.json());
    console.log(runesReforged)

    const iconPrefix = '/static/lol/';

    const currentRunes = {};
    currentRunes.primaryPerkIds = [];
    currentRunes.subPerkIds = [];



    const primaryStyleContainer = document.querySelector('.primary-container .styles-container');
    const primarySlotsContainer = document.querySelector('.primary-container .slots-container');
    const subStyleContainer = document.querySelector('.sub-container .styles-container');
    const subSlotsContainer = document.querySelector('.sub-container .slots-container');

    const updateUI = () => {
        primaryStyleContainer.querySelectorAll('a').forEach(x => {
            if (x.dataset.id != currentRunes.primaryStyle) {
                x.classList.remove('active');
                return;
            }

            x.classList.add('active');
        });

        primarySlotsContainer.querySelectorAll('a').forEach(x => {
            const id = Number(x.dataset.id);
            if (!currentRunes.primaryPerkIds.includes(id)) {
                x.classList.remove('active');
                return;
            }

            x.classList.add('active');
        });

        subStyleContainer.querySelectorAll('a').forEach(x => {
            if (x.dataset.id != currentRunes.subStyle) {
                x.classList.remove('active');
                return;
            }

            x.classList.add('active');
        });

        subSlotsContainer.querySelectorAll('a').forEach(x => {
            const id = Number(x.dataset.id);
            if (!currentRunes.subPerkIds.filter(x => x.id == id).length > 0) {
                x.classList.remove('active');
                return;
            }

            x.classList.add('active');
        });
    }

    const seedPrimaryContainer = (style) => {
        const styleEl = document.createElement('a');
        styleEl.href = '#';
        styleEl.dataset.id = style.id;
        styleEl.addEventListener('click', (e) => {
            e.preventDefault();

            updatePrimaryStyle(style.id);
        });
        const styleImg = document.createElement('img');
        styleImg.alt = style.name;
        styleImg.src = iconPrefix + style.icon;

        styleEl.appendChild(styleImg);
        primaryStyleContainer.appendChild(styleEl);
    };

    const updatePrimaryStyle = (styleId) => {
        const style = runesReforged.find(x => x.id == styleId);

        currentRunes.primaryStyle = style.id;

        primarySlotsContainer.innerHTML = '';

        style.slots.forEach((slot, row) => {
            const slotEl = document.createElement('div');
            slotEl.className = 'slot';

            slot.runes.forEach(rune => {
                const runeEl = document.createElement('a');
                runeEl.dataset.id = rune.id;
                slotEl.appendChild(runeEl);
                runeEl.href = '#';
                runeEl.addEventListener('click', (e) => {
                    e.preventDefault();

                    currentRunes.primaryPerkIds[row] = rune.id;
                    updateUI();
                    console.log(currentRunes);
                });

                const runeImg = document.createElement('img');
                runeEl.appendChild(runeImg);
                runeImg.src = iconPrefix + rune.icon;
                runeImg.alt = rune.name;
            });

            primarySlotsContainer.appendChild(slotEl);
        });

        updateUI();
    };

    const seedSubContainer = (style) => {
        const styleEl = document.createElement('a');
        styleEl.dataset.id = style.id;
        styleEl.href = '#';
        styleEl.addEventListener('click', (e) => {
            e.preventDefault();

            updateSubStyle(style.id);
        });
        const styleImg = document.createElement('img');
        styleImg.alt = style.name;
        styleImg.src = iconPrefix + style.icon;

        styleEl.appendChild(styleImg);
        subStyleContainer.appendChild(styleEl);
    };

    const updateSubStyle = (styleId) => {
        const style = runesReforged.find(x => x.id == styleId);

        currentRunes.subStyle = style.id;

        subSlotsContainer.innerHTML = '';

        style.slots.slice(1).forEach((slot, row) => {
            const slotEl = document.createElement('div');
            slotEl.className = 'slot';

            slot.runes.forEach(rune => {
                const runeEl = document.createElement('a');
                runeEl.dataset.id = rune.id;
                slotEl.appendChild(runeEl);
                runeEl.href = '#';
                runeEl.addEventListener('click', (e) => {
                    e.preventDefault();

                    const runeFromSameRow = currentRunes.subPerkIds.find(x => x.row == row);

                    if (runeFromSameRow && runeFromSameRow.id != rune.id) {
                        currentRunes.subPerkIds = [...currentRunes.subPerkIds.filter(x => x.id != runeFromSameRow.id), { id: rune.id, row: row }];
                        updateUI();
                        return;
                    }

                    if (currentRunes.subPerkIds.length > 1) {
                        currentRunes.subPerkIds.shift();
                    }

                    currentRunes.subPerkIds.push({ id: rune.id, row: row });
                    updateUI();
                    console.log(currentRunes);
                });

                const runeImg = document.createElement('img');
                runeEl.appendChild(runeImg);
                runeImg.src = iconPrefix + rune.icon;
                runeImg.alt = rune.name;
            });

            subSlotsContainer.appendChild(slotEl);
        });

        updateUI();
    };

    runesReforged.forEach(style => {
        seedPrimaryContainer(style);
        seedSubContainer(style);
    });
})();