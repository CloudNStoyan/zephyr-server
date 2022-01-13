(async () => {

    const runesReforged = await fetch('/js/lol/runesReforged.json').then(resp => resp.json());
    const statMods = await fetch('/js/lol/statMods.json').then(resp => resp.json());

    const iconPrefix = '/static/lol/';

    const currentRunes = {};
    currentRunes.primaryPerkIds = [];
    currentRunes.subPerkIds = [];
    currentRunes.statMods = [];

    const primaryStyleContainer = document.querySelector('.primary-container .styles-container');
    const primarySlotsContainer = document.querySelector('.primary-container .slots-container');
    const subStyleContainer = document.querySelector('.sub-container .styles-container');
    const subSlotsContainer = document.querySelector('.sub-container .slots-container');
    const statContainer = document.querySelector('.stat-container');

    const primaryStyleIdInput = document.querySelector('#PrimaryStyleId');
    const subStyleIdInput = document.querySelector('#SubStyleId');
    const perkIdsInput = document.querySelector('#PerkIds');

    const updateDom = () => {
        primaryStyleIdInput.value = currentRunes.primaryStyle;
        subStyleIdInput.value = currentRunes.subStyle;
        perkIdsInput.value = [...currentRunes.primaryPerkIds, ...currentRunes.subPerkIds.map(x => x.id), ...currentRunes.statMods].join(' ');

        statContainer.querySelectorAll('div').forEach((slotEl, row) => {
            const statEls = slotEl.querySelectorAll('a');
            statEls.forEach(statEl => {
                const id = Number(statEl.dataset.id);

                if (currentRunes.statMods[row] != id) {
                    statEl.classList.remove('active');
                    return;
                }

                statEl.classList.add('active');
            });
        });

        primaryStyleContainer.querySelectorAll('a').forEach(x => {
            const id = Number(x.dataset.id);
            if (id !== currentRunes.primaryStyle) {
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
            const id = Number(x.dataset.id);
            if (id !== currentRunes.subStyle) {
                x.classList.remove('active');
                return;
            }

            x.classList.add('active');
        });

        subSlotsContainer.querySelectorAll('a').forEach(x => {
            const id = Number(x.dataset.id);
            if (!currentRunes.subPerkIds.filter(x => x.id === id).length > 0) {
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

            selectPrimaryStyle(style.id);
        });
        const styleImg = document.createElement('img');
        styleImg.alt = style.name;
        styleImg.src = iconPrefix + style.icon;

        styleEl.appendChild(styleImg);
        primaryStyleContainer.appendChild(styleEl);
    };

    const selectPrimaryStyle = (styleId) => {
        updatePrimaryStyle(styleId);

        subStyleContainer.innerHTML = '';
        runesReforged.filter(x => x.id != styleId).forEach(x => seedSubContainer(x));
    }

    const updatePrimaryStyle = (styleId) => {
        const style = runesReforged.find(x => x.id === styleId);

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
                    updateDom();
                });

                const runeImg = document.createElement('img');
                runeEl.appendChild(runeImg);
                runeImg.src = iconPrefix + rune.icon;
                runeImg.alt = rune.name;
            });

            primarySlotsContainer.appendChild(slotEl);
        });

        updateDom();
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
        const style = runesReforged.find(x => x.id === styleId);

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

                    const runeFromSameRow = currentRunes.subPerkIds.find(x => x.row === row);

                    if (runeFromSameRow && runeFromSameRow.id === rune.id) {
                        return;
                    }

                    if (runeFromSameRow && runeFromSameRow.id !== rune.id) {
                        currentRunes.subPerkIds = [...currentRunes.subPerkIds.filter(x => x.id !== runeFromSameRow.id), { id: rune.id, row: row }];
                        updateDom();
                        return;
                    }

                    if (currentRunes.subPerkIds.length > 1) {
                        currentRunes.subPerkIds.shift();
                    }

                    currentRunes.subPerkIds.push({ id: rune.id, row: row });
                    updateDom();
                    console.log(currentRunes);
                });

                const runeImg = document.createElement('img');
                runeEl.appendChild(runeImg);
                runeImg.src = iconPrefix + rune.icon;
                runeImg.alt = rune.name;
            });

            subSlotsContainer.appendChild(slotEl);
        });

        updateDom();
    };

    const seedStatModsContainer = (statMods) => {
        statContainer.innerHTML = '';
        statMods.forEach((slots, row) => {
            const slotEl = document.createElement('div');

            slots.forEach(stat => {
                const statEl = document.createElement('a');
                slotEl.appendChild(statEl);
                statEl.href = '#';
                statEl.dataset.id = stat.id;
                statEl.addEventListener('click', (e) => {
                    e.preventDefault();

                    currentRunes.statMods[row] = stat.id;
                    updateDom();
                });

                const statImgEl = document.createElement('img');
                statEl.appendChild(statImgEl);
                statImgEl.src = iconPrefix + stat.icon;
                statImgEl.alt = stat.name;

            });

            statContainer.appendChild(slotEl);
        });
    };

    const findRowOfRune = (style, id) => {
        const slots = style.slots.slice(1);

        for (let i = 0; i < slots.length; i++) {
            const runes = slots[i].runes;

            if (runes.find(x => x.id === id)) {
                return i;
            }
        }

        return -1;
    }

    const applyRunePageFromFormFields = () => {
        const primaryStyleId = Number(primaryStyleIdInput.value);
        console.log(primaryStyleId)
        if (!isNaN(primaryStyleId)) {
            currentRunes.primaryStyle = primaryStyleId;
        }

        const subStyleId = Number(subStyleIdInput.value);
        console.log(subStyleIdInput.value)
        if (!isNaN(subStyleId)) {
            currentRunes.subStyle = subStyleId;
        }

        let perkIds = [];

        if (perkIdsInput.value.toString().length > 0) {
            perkIds = perkIdsInput.value.split(' ').map(x => Number(x));
        }

        if (!isNaN(primaryStyleId)) {
            selectPrimaryStyle(primaryStyleId);
        }

        if (!isNaN(subStyleId)) {
            updateSubStyle(subStyleId);
        }

        if (perkIds.length > 0) {
            const primaryPerkIds = perkIds.slice(0, 4);
            let subPerkIds = perkIds.slice(4, 6);
            const statMods = perkIds.slice(6, 9);

            const subStyle = runesReforged.find(x => x.id === subStyleId);

            subPerkIds = subPerkIds.map(id => {
                return { id: id, row: findRowOfRune(subStyle, id) };
            });

            currentRunes.primaryPerkIds = primaryPerkIds;
            currentRunes.subPerkIds = subPerkIds;
            currentRunes.statMods = statMods;
            updateDom();
        }
    }

    runesReforged.forEach(style => {
        seedPrimaryContainer(style);
    });

    seedStatModsContainer(statMods);

    applyRunePageFromFormFields();
})();