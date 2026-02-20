
document.getElementById("lessonDate").addEventListener("change", function () {
    const selectedDate = this.value;

    document.querySelectorAll(".attendance-date-hidden")
        .forEach(input => {
            input.value = selectedDate;
        });
});
document.querySelectorAll(".student-row").forEach(row => {
    const attendanceInput = row.querySelector(".att-input");
    const buttons = row.querySelectorAll(".att-btn");

    buttons.forEach(btn => {
        btn.addEventListener("click", () => {

            
            attendanceInput.value = btn.dataset.value;

            
            buttons.forEach(b => b.classList.remove("active"));
            btn.classList.add("active");

        });
    });
});