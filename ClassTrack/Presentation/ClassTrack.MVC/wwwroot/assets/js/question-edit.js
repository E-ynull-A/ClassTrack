(() => {
    const wrap = document.getElementById("optionsWrap");
    const btnAdd = document.getElementById("btnAddOption");
    if (!wrap || !btnAdd) return;

    let index = parseInt(wrap.dataset.startIndex || "0", 10);


    btnAdd.addEventListener("click", () => {
        const row = document.createElement("div");
        row.className = "opt-row";
        row.innerHTML = `
                <div class="opt-left">
                    <input type="hidden" name="PutChoice.Options[${index}].IsDeleted" value="false" class="opt-deleted" />
                    <input type="hidden" name="PutChoice.Options[${index}].Id" value="999" />
                    <input class="input opt-variant" name="PutChoice.Options[${index}].Variant" placeholder="Variant mətni..." required />
        
                    <label class="row mini">
                           <input class="opt-correct"
                                        asp-for="PutChoice.Options[i].IsCorrect" />
                                    <span>Düzgün</span>
                    </label>
                </div>
                <button class="icon-btn danger" type="button" data-remove>✕</button>
            `;
        wrap.appendChild(row);
        index++;
    });

    
    wrap.addEventListener("click", (e) => {
        const btn = e.target.closest("[data-remove]");
        if (!btn) return;

        const row = btn.closest(".opt-row");
        const deletedInput = row.querySelector(".opt-deleted");

        if (deletedInput) {
            deletedInput.value = "true"; 
            row.style.display = "none";  

         
            const variantInput = row.querySelector(".opt-variant");
            if (variantInput) variantInput.removeAttribute("required");
        }
    });
})();