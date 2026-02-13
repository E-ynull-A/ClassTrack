document.querySelectorAll(".student-row").forEach(row => {
    const input = row.querySelector(".att-input");
    row.querySelectorAll(".att-btn").forEach(btn =>
        btn.onclick = () => input.value = btn.dataset.value
    );
});