namespace SchoolRegister.Models; 

public class NoteService : INoteService {
    private readonly SchoolRegisterContext registerContext;

    public NoteService(SchoolRegisterContext registerContext) {
        this.registerContext = registerContext;
    }
}