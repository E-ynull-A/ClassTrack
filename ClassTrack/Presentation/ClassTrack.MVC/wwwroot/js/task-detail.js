// task-detail.js
(() => {
  const role = (document.getElementById("roleBadge")?.textContent || "").trim().toLowerCase();

  const teacherBox = document.getElementById("teacherBox");
  const studentBox = document.getElementById("studentBox");

  const btnSaveTeacher = document.getElementById("btnSaveTeacher");
  const btnSubmitStudent = document.getElementById("btnSubmitStudent");

  const deadlineInput = teacherBox?.querySelector('input[name="Deadline"]');
  const deadlineText = document.getElementById("deadlineText");
  const closedBadge = document.getElementById("closedBadge");

  function readDeadline(){
    // Teacher input varsa ordan, yoxdursa deadlineText-dən oxuyuruq
    if (deadlineInput?.value) {
      // "2026-01-30T23:59"
      return new Date(deadlineInput.value);
    }
    // fallback: parse simple text (best-effort)
    const t = (deadlineText?.textContent || "").trim().replace(" ", "T");
    const d = new Date(t);
    return isNaN(d.getTime()) ? null : d;
  }

  function applyClosedUI(){
    const d = readDeadline();
    if (!d) return;

    const now = new Date();
    const closed = now.getTime() > d.getTime();

    if (closedBadge) closedBadge.style.display = closed ? "inline-flex" : "none";

    // student submit bağla
    if (role === "student") {
      if (btnSubmitStudent) btnSubmitStudent.disabled = closed;
      // form elementlərini də disable edə bilərsən
      document.querySelectorAll("#studentForm .input, #studentForm .textarea, #studentForm button")
        .forEach(el => { if (el.id !== "btnSubmitStudent") el.disabled = closed; });
    }
  }

  // Role-based layout
  if (role === "teacher") {
    if (studentBox) studentBox.style.display = "none";
    if (btnSubmitStudent) btnSubmitStudent.style.display = "none";

    btnSaveTeacher?.addEventListener("click", () => {
      toast("Save", "MVC-də teacher task save POST edəcəksən.");
    });

    document.getElementById("btnUploadTaskFile")?.addEventListener("click", () => {
      toast("Upload", "MVC-də file upload input bağlayacaqsan.");
    });

    document.getElementById("btnDeleteTask")?.addEventListener("click", () => {
      toast("Delete", "MVC-də delete POST/DELETE edəcəksən.");
    });

    deadlineInput?.addEventListener("change", () => {
      // UI-da deadlineText-i update et (demo)
      if (deadlineText) {
        const v = deadlineInput.value.replace("T", " ");
        deadlineText.textContent = v;
      }
      applyClosedUI();
    });

  } else {
    if (teacherBox) teacherBox.style.display = "none";
    if (btnSaveTeacher) btnSaveTeacher.style.display = "none";

    btnSubmitStudent?.addEventListener("click", () => {
      toast("Submit", "MVC-də student submit POST edəcəksən.");
    });

    document.getElementById("btnAttachStudent")?.addEventListener("click", () => {
      toast("Attach", "MVC-də file attach edəcəksən.");
    });
  }

  applyClosedUI();
})();
