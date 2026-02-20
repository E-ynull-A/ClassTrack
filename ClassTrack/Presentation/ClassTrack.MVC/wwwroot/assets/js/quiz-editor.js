(() => {
  const host = $("#questions");
  const tplQ = $("#tplQuestion");
  const tplC = $("#tplChoice");

  function addChoice(qEl){
    const list = $(".choice-list", qEl);
    const c = tplC.content.cloneNode(true);
    const row = c.querySelector(".choice");
    row.querySelector("[data-act='removeChoice']").addEventListener("click", () => row.remove());
    list.appendChild(c);
  }

  function addQuestion(){
    const frag = tplQ.content.cloneNode(true);
    const qEl = frag.querySelector(".qcard");

    // default 2 choice
    addChoice(qEl); addChoice(qEl);

    qEl.querySelector("[data-act='addChoice']").addEventListener("click", () => addChoice(qEl));
    qEl.querySelector("[data-act='removeQ']").addEventListener("click", () => qEl.remove());

    // type change: hide choices if not MCQ
    const typeSel = $(".qType", qEl);
    const choicesBox = $(".choices", qEl);
    typeSel.addEventListener("change", () => {
      choicesBox.style.display = (typeSel.value === "MCQ") ? "" : "none";
    });

    host.appendChild(qEl);
  }

  $("#btnAddQ").addEventListener("click", addQuestion);

  // demo initial question
  addQuestion();

  $("#btnSave").addEventListener("click", () => {
    toast("Save", "Bunu MVC-də POST edəcəksən. (QuizEditorVM)");
  });
})();
