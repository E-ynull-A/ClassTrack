document.getElementById("forgotForm").addEventListener("submit", function (e) {
    e.preventDefault();

    const email = document.getElementById("Email").value;
    const emailError = document.getElementById("emailError");

    emailError.textContent = "";

    if (!email.includes("@")) {
        emailError.textContent = "Please enter valid email";
        return;
    }

    alert("Reset link sent to your email (demo)");
});