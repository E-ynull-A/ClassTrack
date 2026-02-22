namespace ClassTrack.MVC.ViewModels
{
    public record TaskEvaulateVM(

        GetTaskWorkVM? GetTaskWork,
        GetStudentTaskWorkVM? StudentAnswer,
        PutPointInTaskWorkVM PutPointInTask = null);
  
}
