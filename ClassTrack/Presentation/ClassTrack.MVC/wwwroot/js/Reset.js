document.getElementById("resetForm").addEventListener("submit", function (e) {
    e.preventDefault();

    const pass = document.getElementById("password").value;
    const confirm = document.getElementById("confirmPassword").value;
    const error = document.getElementById("passError");

    error.textContent = "";

    if (pass.length < 6) {
        error.textContent = "Password must be at least 6 characters";
        return;
    }

    if (pass !== confirm) {
        error.textContent = "Passwords do not match";
        return;
    }

    alert("Password updated successfully (demo)");
});