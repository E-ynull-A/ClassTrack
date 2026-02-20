(() => {
    const grid = document.getElementById("quizGrid");
    const empty = document.getElementById("qEmpty");
    const search = document.getElementById("qSearch");
    const sort = document.getElementById("qSort");
    if (!grid) return;

    const cards = Array.from(grid.querySelectorAll(".quiz-card"));

    function getData(card) {
        const name = (card.querySelector(".qc-name")?.textContent || "").trim();
        const idTxt = (card.querySelector(".badge")?.textContent || "").replace("ID:", "").trim();
        const id = parseInt(idTxt || "0", 10);

        const startRaw = card.querySelector(".qc-start")?.getAttribute("data-start") || "";
        const start = startRaw ? new Date(startRaw).getTime() : 0;

        const durMin = parseInt(card.querySelector(".qc-duration")?.getAttribute("data-min") || "0", 10);

        return { name, id, start, durMin };
    }

    function apply() {
        const q = (search?.value || "").trim().toLowerCase();
        const mode = sort?.value || "start_asc";

        // filter
        let list = cards.filter(c => {
            if (!q) return true;
            const d = getData(c);
            return d.name.toLowerCase().includes(q) || String(d.id).includes(q);
        });

        // sort
        list.sort((a, b) => {
            const A = getData(a), B = getData(b);
            switch (mode) {
                case "start_desc": return (B.start - A.start);
                case "name_asc": return A.name.localeCompare(B.name);
                case "name_desc": return B.name.localeCompare(A.name);
                default: return (A.start - B.start); // start_asc
            }
        });

        // re-render order + hide others
        cards.forEach(c => c.style.display = "none");
        list.forEach(c => {
            c.style.display = "";
            grid.appendChild(c); // reorder
        });

        if (empty) empty.style.display = list.length ? "none" : "block";
        grid.classList.toggle("single", list.length === 1);
    }

    search?.addEventListener("input", apply);
    sort?.addEventListener("change", apply);


    apply();
})();
