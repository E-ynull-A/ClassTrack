(() => {
  const email = $("#email");
  const pass = $("#password");

  $("#btnDemo").addEventListener("click", () => {
    email.value = "teacher@test.com";
    pass.value = "123456";
    toast("Demo", "Inputlar dolduruldu.");
  });

  $("#btnLogin").addEventListener("click", () => {
    // Burda sən MVC form post edəcəksən.
    if(!email.value || !pass.value){
      toast("Xəta", "Email və şifrə boş ola bilməz.");
      return;
    }
    toast("OK", "Bunu MVC-də Auth/Login POST-ə bağla.");
  });
})();
