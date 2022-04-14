using Structurizr;
using Structurizr.Api;

namespace c4_model_design
{
    class Program
    {
        static void Main(string[] args)
        {
            Banking();
        }

        static void Banking()
        {
            const long workspaceId = 69924;
            const string apiKey = "7ba302b5-585c-4ea5-a0fa-d2299052ebba";
            const string apiSecret = "dfd208d8-c8b7-409a-b727-c99a51a377c7";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            Workspace workspace = new Workspace("Software Design - C4 Model - PsychoHelp", "Sodtware Architecture Design PsychoHelp");
            Model model = workspace.Model;

            SoftwareSystem psychoPlatform = model.AddSoftwareSystem("PsychoHelp Plataform", "Permite a los usuarios visualizar información y contactar a los psicologos disponibles");
            SoftwareSystem collegeApi = model.AddSoftwareSystem("CPSP API", "API del colegio de psicologos para verificar la informacion de los psicologos registrados.");
            SoftwareSystem paypalApi = model.AddSoftwareSystem("PayPal API", "API de la empresa PayPal que permite pagos de manera online");
            SoftwareSystem zoomApi = model.AddSoftwareSystem("Zoom API", "API de la empresa Zoom que permite servicios de videollamada");

            Person patient = model.AddPerson("Patient", "Usuario de la Plataforma.");
            Person psychologist = model.AddPerson("Psychologist", "Usuario que presta sus servicios profesionales mediante la plataforma");

            patient.Uses(psychoPlatform, "Usa");
            psychologist.Uses(psychoPlatform, "Usa");
            paypalApi.Uses(psychoPlatform, "Permite los pagos dentro de la plataforma");
            zoomApi.Uses(psychoPlatform, "Permite videollamadas dentro de la plataforma");
            psychoPlatform.Uses(collegeApi, "Consulta la información del psicologo");
            
            ViewSet viewSet = workspace.Views;

            // 1. Diagrama de Contexto
            SystemContextView contextView = viewSet.CreateSystemContextView(psychoPlatform, "Contexto", "Diagram de Contexto");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople(); 

            // Tags
            psychoPlatform.AddTags("PlataformaPsico");
            collegeApi.AddTags("ColegioPsico");
            paypalApi.AddTags("PayPalA");
            zoomApi.AddTags("ZoomA");
            patient.AddTags("Paciente");
            psychologist.AddTags("Psicologo");
            
            //Styles
            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("Paciente") { Background = "#03A9F4", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("Psicologo") { Background = "#03A9F4", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("PlataformaPsico") { Background = "#009688", Color = "#ffffff", Shape = Shape.RoundedBox });            
            styles.Add(new ElementStyle("ColegioPsico") { Background = "#448AFF", Color = "#ffffff", Shape = Shape.RoundedBox });            
            styles.Add(new ElementStyle("PayPalA") { Background = "#FF5722", Color = "#ffffff", Shape = Shape.RoundedBox });            
            styles.Add(new ElementStyle("ZoomA") { Background = "#E1BEE7", Color = "#ffffff", Shape = Shape.RoundedBox });
            
            
            
            // 2. Diagrama de Contenedores
            
            Container webApplication = psychoPlatform.AddContainer("Web Application", "Entrega contenido estático y la página simple de PsychoHelp Platform", "Spring Java");
            Container singlePageApplication = psychoPlatform.AddContainer("Single Page Aplicattion", "Provee toda la funcionalidad de la plataforma de PsychoHelp a los pacientes y psicólogos", "Angular");
            Container springBootApi = psychoPlatform.AddContainer("SpringBoot API", "Permite a los usuarios de la aplicación conectarse e interactuar, compartiendo y agregando información", "");
            
            Container appointmentBoundedContext = psychoPlatform.AddContainer("Appointment Bounded Context", "Bounded Context que permite el registro de citas entre pacientes y psicólogos", "");
            Container accountBoundedContext = psychoPlatform.AddContainer("Account Bounded Context", "Bounded Context que gestiona las cuentas de los psicólogos", "");
            Container billingBoundedContext = psychoPlatform.AddContainer("Billing Bounded Context", "Bounded Context que gestiona la información de facturación", "");
            Container psychologistBoundedContext = psychoPlatform.AddContainer("Psychologist Bounded Context", "Bounded Context que permite registrar a los psicólogos", "");
            Container patientBoundedContext = psychoPlatform.AddContainer("Patient Bounded Context", "Bounded Context que permite registrar a los pacientes", "");
            Container paymentBoundedContext = psychoPlatform.AddContainer("Payment Bounded Context", "Bounded Context que gestiona los pagos en la plataforma", "");
            Container logbookBoundedContext = psychoPlatform.AddContainer("Logbook Bounded Context", "Bounded Context que permite registrar diagnósticos", "");
            
            Container dataBase = psychoPlatform.AddContainer("Data Base", "Permite el almacenamiento de información", "PostgreSQL");

            patient.Uses(webApplication, "Buscar recomendaciones y consejos sobre la salud mental usando");
            psychologist.Uses(webApplication, "Prestar sus servicios como profesional en psicología usando");

            webApplication.Uses(singlePageApplication, "Entrega al navegador web del cliente");
            singlePageApplication.Uses(springBootApi, "Usa");
            singlePageApplication.Uses(collegeApi, "Verifica los datos de los psicólogos");

            singlePageApplication.Uses(appointmentBoundedContext, "Llamada API a");
            singlePageApplication.Uses(accountBoundedContext, "Llamada API a");
            singlePageApplication.Uses(billingBoundedContext, "Llamada API a");
            singlePageApplication.Uses(psychologistBoundedContext, "Llamada API a");
            singlePageApplication.Uses(patientBoundedContext, "Llamada API a");
            singlePageApplication.Uses(paymentBoundedContext, "Llamada API a");
            singlePageApplication.Uses(logbookBoundedContext, "Llamada API a");

            appointmentBoundedContext.Uses(dataBase, "Lee desde y Escribe a");
            accountBoundedContext.Uses(dataBase, "Lee desde y Escribe a");
            billingBoundedContext.Uses(dataBase, "Lee desde y Escribe a");
            psychologistBoundedContext.Uses(dataBase, "Lee desde y Escribe a");
            patientBoundedContext.Uses(dataBase, "Lee desde y Escribe a");
            paymentBoundedContext.Uses(dataBase, "Lee desde y Escribe a");
            logbookBoundedContext.Uses(dataBase, "Lee desde y Escribe a");

            zoomApi.Uses(appointmentBoundedContext, "Permite la elaboración de videollamadas");
            paypalApi.Uses(paymentBoundedContext, "Permite los pagos por los servicios ofrecidos");

            //Tags
            webApplication.AddTags("WebApp");
            singlePageApplication.AddTags("PageApp");
            springBootApi.AddTags("SpringAPI");
            
            appointmentBoundedContext.AddTags("Appointment");
            accountBoundedContext.AddTags("Account");
            billingBoundedContext.AddTags("Billing");
            psychologistBoundedContext.AddTags("Psycho");
            patientBoundedContext.AddTags("Patient");
            paymentBoundedContext.AddTags("Payment");
            logbookBoundedContext.AddTags("Logbook");
            dataBase.AddTags("DataBase");
            
            //Styles
            styles.Add(new ElementStyle("WebApp") { Background = "#E64A19", Color = "#ffffff", Shape = Shape.WebBrowser});
            styles.Add(new ElementStyle("PageApp") { Background = "#7C4DFF", Color = "#ffffff", Shape = Shape.RoundedBox});
            styles.Add(new ElementStyle("SpringAPI") { Background = "#7C4DFF", Color = "#ffffff", Shape = Shape.RoundedBox});
            
            styles.Add(new ElementStyle("Appointment") { Background = "#F8BBD0", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("Account") { Background = "#F8BBD0", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("Billing") { Background = "#F8BBD0", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("Treatment") { Background = "#F8BBD0", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("Psycho") { Background = "#F8BBD0", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("Patient") { Background = "#F8BBD0", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("Payment") { Background = "#F8BBD0", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("Professional") { Background = "#F8BBD0", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("Logbook") { Background = "#F8BBD0", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("Recommendations") { Background = "#F8BBD0", Color = "#ffffff", Shape = Shape.Hexagon });    
            styles.Add(new ElementStyle("Specialty") { Background = "#F8BBD0", Color = "#ffffff", Shape = Shape.Hexagon });    
            styles.Add(new ElementStyle("DataBase") { Background = "#B71DDE", Color = "#ffffff", Shape = Shape.Cylinder });

            ContainerView containerView = viewSet.CreateContainerView(psychoPlatform, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();  
            
            //Diagrama de componentes Appointment BC
            Component appointmentController = appointmentBoundedContext.AddComponent("Appointment Controller", "Controlador que provee los Rest API para la gestión de citas", "");
            Component appointmentService = appointmentBoundedContext.AddComponent("Appointment Service", "Provee los métodos para la inscripción y gestión de citas", "");
            Component appointmentRepository = appointmentBoundedContext.AddComponent("Appointment Repository", "Repositorio que provee los métodos para la persistencia de los datos de las citas.", "");
            Component appointmentDomain = appointmentBoundedContext.AddComponent("Appointment Domain Model", "Contiene todas las entidades del Bounded Context", "");

            singlePageApplication.Uses(appointmentController, "Llamada API a");
            appointmentController.Uses(appointmentService, "Llamada a los métodos del service");
            appointmentService.Uses(appointmentRepository, "Llamada a los métodos de persistencia del repository");
            appointmentDomain.Uses(appointmentRepository, "Conforma");
            appointmentRepository.Uses(dataBase, "Lee desde y Escribe a");
            appointmentService.Uses(zoomApi, "Usa");
            
            //Tags
            appointmentController.AddTags("appointmentController");
            appointmentService.AddTags("appointmentService");
            appointmentRepository.AddTags("appointmentRepository");
            appointmentDomain.AddTags("appointmentDomain");
            
            styles.Add(new ElementStyle("appointmentController") {Background = "#234DFF", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("appointmentService") {Background = "#234DFF", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("appointmentRepository") {Background = "#234DFF", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("appointmentDomain") {Background = "#234DFF", Color = "#ffffff", Shape = Shape.Component});

            ComponentView componentView = viewSet.CreateComponentView(appointmentBoundedContext, "Components", "Component Diagram");
            componentView.PaperSize = PaperSize.A4_Landscape;
            componentView.Add(appointmentBoundedContext);
            componentView.Add(singlePageApplication);
            componentView.Add(webApplication);
            componentView.Add(dataBase);
            componentView.Add(zoomApi);
            componentView.AddAllComponents();
            
            //Diagrama de componentes Account BC
            Component accountController = accountBoundedContext.AddComponent("Account Controller", "Controlador que se encarga de obtener las cuentas del psicólogo", "");
            Component accountService = accountBoundedContext.AddComponent("Account Service", "Provee los métodos para la gestión de cuentas de los psicólogos", "");
            Component accountValidation = accountBoundedContext.AddComponent("Account Validation", "Valida los datos de las cuentas", "");
            Component accountRepository = accountBoundedContext.AddComponent("Account Repository", "Provee los métodos para la persistencia de los datos de las cuentas del psicólogo", "");
            Component accountDomain = accountBoundedContext.AddComponent("Account Domain Model", "Contiene todas las entidades del Bounded Context", "");

            singlePageApplication.Uses(accountController, "Llamada API a");
            accountController.Uses(accountService, "Llamada a los métodos del service");
            accountService.Uses(accountRepository, "Llamada a los métodos de persistencia del repository");
            accountService.Uses(accountValidation, "Llamada a los métodos de validación");
            accountDomain.Uses(accountRepository, "Conforma");
            accountRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            //Tags
            accountController.AddTags("accountController");
            accountService.AddTags("accountService");
            accountRepository.AddTags("accountRepository");
            accountValidation.AddTags("accountValidation");
            accountDomain.AddTags("accountDomain");

            styles.Add(new ElementStyle("accountController") {Background = "#E0132E", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("accountService") {Background = "#E0132E", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("accountRepository") {Background = "#E0132E", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("accountValidation") {Background = "#E0132E", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("accountDomain") {Background = "#E0132E", Color = "#ffffff", Shape = Shape.Component});
 
            ComponentView componentView2 = viewSet.CreateComponentView(accountBoundedContext, "Components2", "Component Diagram");
            componentView2.PaperSize = PaperSize.A4_Landscape;
            componentView2.Add(accountBoundedContext);
            componentView2.Add(singlePageApplication);
            componentView2.Add(webApplication);
            componentView2.Add(dataBase);
            componentView2.AddAllComponents();
            
            //Diagrama de componentes Payment BC
            Component paymentController = paymentBoundedContext.AddComponent("Payment Controller", "Controlador que provee los RestAPI para la gestión de pagos", "");
            Component paymentService = paymentBoundedContext.AddComponent("Payment Service", "Provee los métodos para la realización de los pagos", "");
            Component payPalService = paymentBoundedContext.AddComponent("PayPal Service", "Servicio encargado de conectarse con la plataforma de PayPal para verificar el pago del cliente", "");
            Component paymentRepository = paymentBoundedContext.AddComponent("Payment Repository", "Provee los métodos para la persistencia de los datos de los pagos", "");
            Component paymentDomain = paymentBoundedContext.AddComponent("Payment Domain Model", "Contiene todas las entidades del Bounded Context", "");

            singlePageApplication.Uses(paymentController, "Llamada API a");
            paymentController.Uses(paymentService, "Llamada a los métodos del service");
            paymentController.Uses(payPalService, "Llamada a los métodos del service");
            payPalService.Uses(paypalApi, "Usa");
            paymentService.Uses(paymentRepository, "Llamada a los métodos de persistencia del repository");
            paymentDomain.Uses(paymentRepository, "Conforma");
            paymentRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            //Tags
            paymentController.AddTags("paymentController");
            paymentService.AddTags("paymentService");
            payPalService.AddTags("payPalService");
            paymentRepository.AddTags("paymentRepository");
            paymentDomain.AddTags("paymentDomain");
            
            styles.Add(new ElementStyle("paymentController") {Background = "#34FF2C", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("paymentService") {Background = "#34FF2C", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("payPalService") {Background = "#34FF2C", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("paymentRepository") {Background = "#34FF2C", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("paymentDomain") {Background = "#34FF2C", Color = "#ffffff", Shape = Shape.Component});
 
            ComponentView componentView3 = viewSet.CreateComponentView(paymentBoundedContext, "Components3", "Component Diagram");
            componentView3.PaperSize = PaperSize.A4_Landscape;
            componentView3.Add(paymentBoundedContext);
            componentView3.Add(singlePageApplication);
            componentView3.Add(webApplication);
            componentView3.Add(paypalApi);
            componentView3.Add(dataBase);
            componentView3.AddAllComponents();
            
            //Diagrama de componentes Logbook BC
            Component logbookController = logbookBoundedContext.AddComponent("Logbook Controller", "Controlador principalmente encargado de obtener la información del Logbook");
            Component logbookService = logbookBoundedContext.AddComponent("Logbook Service", "Provee los métodos para la gestión de logbooks del usuario");
            Component logbookRepository = logbookBoundedContext.AddComponent("Logbook Repository", "Provee los métodos para la persistencia de los datos del logbook");
            Component logbookDomain = logbookBoundedContext.AddComponent("Logbook Domain Model", "Contiene todas las entidades del Bounded Context");

            singlePageApplication.Uses(logbookController, "Llamada API a");
            logbookController.Uses(logbookService, "Llamada a los métodos service");
            logbookService.Uses(logbookRepository, "Llamada a los métodos de persistencia del repository");
            logbookDomain.Uses(logbookRepository, "Conforma");
            logbookRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            //Tags
            logbookController.AddTags("logbookController");
            logbookService.AddTags("logbookService");
            logbookRepository.AddTags("logbookRepository");
            logbookDomain.AddTags("logbookDomain");
            
            styles.Add(new ElementStyle("logbookController") {Background = "#7A005C", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("logbookService") {Background = "#7A005C", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("logbookRepository") {Background = "#7A005C", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("logbookDomain") {Background = "#7A005C", Color = "#ffffff", Shape = Shape.Component});
            
            ComponentView componentView4 = viewSet.CreateComponentView(logbookBoundedContext, "Components4", "Component Diagram");
            componentView4.PaperSize = PaperSize.A4_Landscape;
            componentView4.Add(logbookBoundedContext);
            componentView4.Add(singlePageApplication);
            componentView4.Add(webApplication);
            componentView4.Add(dataBase);
            componentView4.AddAllComponents();
            
            //Diagrama de componentes Patient BC
            Component patientController = patientBoundedContext.AddComponent("Patient Controller", "Provee las RestAPI para el manejo de pacientes");
            Component patientService = patientBoundedContext.AddComponent("Patient Service", "Provee los métodos para la inscripción y gestión de pacientes");
            Component patientRepository = patientBoundedContext.AddComponent("Patient Repository", "Provee los métodos para la persistencia de los datos de los pacientes");
            Component patientDomain = patientBoundedContext.AddComponent("Patient Domain Model", "Contiene todas las entidades del Bounded Context");
            Component patientValidation = patientBoundedContext.AddComponent("Patient Validation", "Se encarga de validar que los datos del paciente son los correctos");

            singlePageApplication.Uses(patientController, "LLamada API a");
            patientController.Uses(patientService, "Llamada a los métodos service");
            patientService.Uses(patientRepository, "Llamada a los métodos de persistencia del repository");
            patientService.Uses(patientValidation, "Llamada a los métodos de validación");
            patientDomain.Uses(patientRepository, "Conforma");
            patientRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            //Tags
            patientController.AddTags("patientController");
            patientService.AddTags("patientService");
            patientRepository.AddTags("patientRepository");
            patientDomain.AddTags("patientDomain");
            patientValidation.AddTags("patientValidation");
            
            styles.Add(new ElementStyle("patientController") {Background = "#FF57DE", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("patientService") {Background = "#FF57DE", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("patientRepository") {Background = "#FF57DE", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("patientDomain") {Background = "#FF57DE", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("patientValidation") {Background = "#FF57DE", Color = "#ffffff", Shape = Shape.Component});
            
            ComponentView componentView5 = viewSet.CreateComponentView(patientBoundedContext, "Components5", "Component Diagram");
            componentView5.PaperSize = PaperSize.A4_Landscape;
            componentView5.Add(patientBoundedContext);
            componentView5.Add(singlePageApplication);
            componentView5.Add(webApplication);
            componentView5.Add(dataBase);
            componentView5.AddAllComponents();
            
            //Diagrama de componentes Psychologist BC
            Component psychoController = psychologistBoundedContext.AddComponent("Psychologist Controller", "Provee las RestAPI para el manejo de psicólogos");
            Component psychoService = psychologistBoundedContext.AddComponent("Psychologist Service", "Provee los métodos para la inscripción y gestión de psicólogos");
            Component psychoRepository = psychologistBoundedContext.AddComponent("Psychologist Repository", "Provee los métodos para la persistencia de los datos de los psicólogos");
            Component psychoDomain = psychologistBoundedContext.AddComponent("Psychologist Domain Model", "Contiene todas las entidades del Bounded Context");
            Component psychoValidation = psychologistBoundedContext.AddComponent("Psychologist Validation", "Se encarga de validar que los datos del psicólogos son los correctos");
            
            singlePageApplication.Uses(psychoController, "LLamada API a");
            psychoController.Uses(psychoService, "Llamada a los métodos service");
            psychoService.Uses(psychoRepository, "Llamada a los métodos de persistencia del repository");
            psychoService.Uses(psychoValidation, "Llamada a los métodos de validación");
            psychoDomain.Uses(psychoRepository, "Conforma");
            psychoRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            //Tags
            psychoController.AddTags("psychoController");
            psychoService.AddTags("psychoService");
            psychoRepository.AddTags("psychoRepository");
            psychoDomain.AddTags("psychoDomain");
            psychoValidation.AddTags("psychoValidation");
            
            styles.Add(new ElementStyle("psychoController") {Background = "#11BFA4", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("psychoService") {Background = "#11BFA4", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("psychoRepository") {Background = "#11BFA4", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("psychoDomain") {Background = "#11BFA4", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("psychoValidation") {Background = "#11BFA4", Color = "#ffffff", Shape = Shape.Component});
            
            ComponentView componentView6 = viewSet.CreateComponentView(psychologistBoundedContext, "Components6", "Component Diagram");
            componentView6.PaperSize = PaperSize.A4_Landscape;
            componentView6.Add(psychologistBoundedContext);
            componentView6.Add(singlePageApplication);
            componentView6.Add(webApplication);
            componentView6.Add(dataBase);
            componentView6.AddAllComponents();
            
            //Diagrama de componentes Billing BC
            Component billController = billingBoundedContext.AddComponent("Billing Controller", "Provee las RestAPI para el manejo de información de facturación");
            Component billService = billingBoundedContext.AddComponent("Billing Application Service", "Provee los métodos para la gestión de información de facturación");
            Component billRepository = billingBoundedContext.AddComponent("Billing Repository", "Provee la persistencia de los datos de la información de facturación");
            Component billValidation = billingBoundedContext.AddComponent("Billing Validation", "Se encarga de validar que los datos de la facturación son los correctos");
            Component billDomain = billingBoundedContext.AddComponent("Billing Domain Model", "Contiene todas las entidades del Bounded Context");

            singlePageApplication.Uses(billController, "Llamada API a");
            billController.Uses(billService, "Llamada a los métodos service");
            billService.Uses(billRepository, "Llamada a los métodos de persistencia del repository");
            billService.Uses(billValidation, "Llamada a los métodos de validación");
            billDomain.Uses(billRepository, "Conforma");
            billRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            //Tags
            billController.AddTags("billController");
            billService.AddTags("billService");
            billRepository.AddTags("billRepository");
            billDomain.AddTags("billDomain");
            billValidation.AddTags("billValidation");
            
            styles.Add(new ElementStyle("billController") {Background = "#C9E028", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("billService") {Background = "#C9E028", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("billRepository") {Background = "#C9E028", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("billDomain") {Background = "#C9E028", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("billValidation") {Background = "#C9E028", Color = "#ffffff", Shape = Shape.Component});
            
            ComponentView componentView7 = viewSet.CreateComponentView(billingBoundedContext, "Components7", "Component Diagram");
            componentView7.PaperSize = PaperSize.A4_Landscape;
            componentView7.Add(billingBoundedContext);
            componentView7.Add(singlePageApplication);
            componentView7.Add(webApplication);
            componentView7.Add(dataBase);
            componentView7.AddAllComponents();

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}