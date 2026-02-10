(() => {
    const optionsWrap = document.getElementById("optionsWrap");
    const btnAdd = document.getElementById("btnAddOption");

    if (!optionsWrap || !btnAdd) return;

    let optionIndex = parseInt(optionsWrap.dataset.startIndex || "0", 10);

    btnAdd.addEventListener("click", () => {
        const row = document.createElement("div");
        row.className = "opt-row";

        row.innerHTML = `
      <div class="opt-left">

        <input type="hidden"
               name="PutChoice.Options[${optionIndex}].IsDeleted"
               value="false"
               class="opt-deleted" />

        <input type="hidden"
               name="PutChoice.Options[${optionIndex}].Id"
               value="" />

        <input class="input opt-variant"
               type="text"
               name="PutChoice.Options[${optionIndex}].Variant"
               placeholder="Variant mətni..." />

        <!-- IsCorrect: hidden(false) + checkbox(true) -->
        <input type="hidden"
               name="PutChoice.Options[${optionIndex}].IsCorrect"
               value="false" />

        <label class="row mini">
          <input class="opt-correct"
                 type="checkbox"
                 name="PutChoice.Options[${optionIndex}].IsCorrect"
                 value="true" />
          <span>Düzgün</span>
        </label>

      </div>

      <button class="icon-btn danger"
              type="button"
              data-remove
              title="Sil">✕</button>
    `;

        optionsWrap.appendChild(row);
        optionIndex++;
    });

    optionsWrap.addEventListener("click", (e) => {
        const btn = e.target.closest("[data-remove]");
        if (!btn) return;

        const row = btn.closest(".opt-row");
        if (!row) return;

        const deletedInput = row.querySelector(".opt-deleted");
        if (deletedInput) deletedInput.value = "true";

        row.style.display = "none";
    });
})();