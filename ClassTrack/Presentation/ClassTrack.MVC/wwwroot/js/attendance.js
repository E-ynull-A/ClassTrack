(() => {
  const role = ($("#roleBadge").textContent || "").trim().toLowerCase();
  const btnAllPresent = $("#btnAllPresent");
  const btnAllAbsent = $("#btnAllAbsent");
  const btnSave = $("#btnSave");

  function setAll(value){
    // value: "0" absent, "1" present, "2" late
    $$(".att-row").forEach(row => {
      const sid = row.dataset.student;
      const radio = row.querySelector(`input[type="radio"][name="att-${sid}"][value="${value}"]`);
      if(radio) radio.checked = true;
    });
  }

  if(role !== "teacher"){
    // student view: disable inputs + hide buttons
    btnAllPresent && (btnAllPresent.style.display = "none");
    btnAllAbsent && (btnAllAbsent.style.display = "none");
    btnSave && (btnSave.style.display = "none");
    $$(".att-row input").forEach(i => i.disabled = true);
    toast("Info", "Student yalnız baxa bilər.");
    return;
  }

  btnAllPresent?.addEventListener("click", () => {
    setAll("1"); // Present
    toast("Done", "Hamısı Present.");
  });

  btnAllAbsent?.addEventListener("click", () => {
    setAll("0"); // Absent
    toast("Done", "Hamısı Absent.");
  });

  btnSave?.addEventListener("click", () => {
    // UI demo: sən bunu MVC form post edəcəksən
    // Məs: hər row üçün StudentId + Status + Note topla.
    toast("Save", "MVC-də Attendance POST edəcəksən.");
  });
})();
