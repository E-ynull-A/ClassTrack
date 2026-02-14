document.querySelectorAll(".student-row").forEach(row => {
    const input = row.querySelector(".att-input");
    row.querySelectorAll(".att-btn").forEach(btn =>
        btn.onclick = () => input.value = btn.dataset.value
    );
});
function forceSyncDates(val) {
    console.log("Seçilən tarix: ", val);
    // Bütün hidden inputları tapırıq
    const targets = document.querySelectorAll('.attendance-date-hidden');

    if (targets.length === 0) {
        console.warn("DİQQƏT: .attendance-date-hidden klası ilə heç bir input tapılmadı!");
    }

    targets.forEach(input => {
        input.value = val;
    });
}

// Form submit olanda hər ehtimala qarşı bir də yoxlayaq
document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector('form');
    if (form) {
        form.onsubmit = function () {
            const dateVal = document.getElementById('lessonDate').value;
            forceSyncDates(dateVal);
        };
    }
});