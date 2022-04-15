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
            Workspace workspace = new Workspace("Software Design - C4 Model - Safe Technology", "Software Architecture Design Safe Technology");
            Model model = workspace.Model;
            
            SoftwareSystem safePlatform = model.AddSoftwareSystem("Safe Technology Platform", "Permite a los usuarios visualizar información y contactar con técnicos disponibles");
            SoftwareSystem stripeApi = model.AddSoftwareSystem("Stripe API", "API de la empresa Paypal que permite pagos de manera online");
            SoftwareSystem emailSystem = model.AddSoftwareSystem("E-mail System", "Sistema de e-mail que envía un correo de confirmación de cuenta");

            Person user = model.AddPerson("User", "Usuario de la plataforma");
            Person technical = model.AddPerson("Technical", "Usuario que presta sus servicios mediante la plataforma");

            user.Uses(safePlatform, "Usa");
            technical.Uses(safePlatform, "Usa");
            safePlatform.Uses(stripeApi, "Permite los pagos dentro de la plataforma");
            safePlatform.Uses(emailSystem, "Envía e-mail de verificación de cuenta");
            emailSystem.Delivers(user, "Envía correo a");
            emailSystem.Delivers(technical, "Envía correo a");
            
            ViewSet viewSet = workspace.Views;

            // 1. Diagrama de Contexto
            SystemContextView contextView = viewSet.CreateSystemContextView(safePlatform, "Contexto", "Diagrama de Contexto");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            safePlatform.AddTags("platformSafe");
            stripeApi.AddTags("stripeA");
            emailSystem.AddTags("emailSystem");
            user.AddTags("user");
            technical.AddTags("technical");

            //Styles
            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("technical") { Background = "#03A9F4", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("user") { Background = "#03A9F4", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("platformSafe") { Background = "#009688", Color = "#ffffff", Shape = Shape.RoundedBox });            
            styles.Add(new ElementStyle("stripeA") { Background = "#6b0023", Color = "#ffffff", Shape = Shape.RoundedBox });            
            styles.Add(new ElementStyle("emailSystem") { Background = "#f568b5", Color = "#ffffff", Shape = Shape.RoundedBox });
            
            
            
            // 2. Diagrama de Contenedores
            
            Container webApplication = safePlatform.AddContainer("Web Application", "Entrega contenido estático y la página simple de Safe Technology Platform", "Spring Java");
            Container singlePageApplication = safePlatform.AddContainer("Single Page Application", "Provee toda la funcionalidad de la plataforma de Safe Technology a los técnicos", "Angular");
            Container springBootApi = safePlatform.AddContainer("API REST", "Permite a los usuarios de la aplicación conectarse e interactuar, compartiendo y agregando información", "SpringBoot");
            
            Container appointmentBoundedContext = safePlatform.AddContainer("Appointment Bounded Context", "Bounded Context que permite el registro de citas técnicas entre usuarios y técnicos", "");
            Container publicationBoundedContext = safePlatform.AddContainer("Publication Bounded Context", "Bounded Context que gestiona las publicaciones de los técnicos", "");
            Container technicalBoundedContext = safePlatform.AddContainer("Technical Bounded Context", "Bounded Context que permite registrar a los técnicos", "");
            Container userBoundedContext = safePlatform.AddContainer("User Bounded Context", "Bounded Context que permite registrar a los usuarios", "");
            Container paymentBoundedContext = safePlatform.AddContainer("Payment Bounded Context", "Bounded Context que gestiona los pagos en la plataforma", "");
            Container reportBoundedContext = safePlatform.AddContainer("Report Bounded Context", "Bounded Context que permite registrar los diagnósticos y reparaciones", "");
            Container applianceCategoryBoundedContext = safePlatform.AddContainer("Appliance Category Bounded Context", "Bounded Context que gestiona las categorías de los electrodomésticos", "");
 
            
            Container dataBase = safePlatform.AddContainer("Data Base", "Permite el almacenamiento de información", "MySQL");

            user.Uses(webApplication, "Buscar recomendaciones y consejos sobre reparaciones de electrodomésticos");
            technical.Uses(webApplication, "Prestar sus servicios como técnico usando la plataforma");

            webApplication.Uses(singlePageApplication, "Entrega al navegador web del cliente");
            singlePageApplication.Uses(springBootApi, "Usa");
            springBootApi.Uses(emailSystem, "Envía correo de verificación");
        

            springBootApi.Uses(appointmentBoundedContext, "Llamada API a");
            springBootApi.Uses(publicationBoundedContext, "Llamada API a");
            springBootApi.Uses(technicalBoundedContext, "Llamada API a");
            springBootApi.Uses(userBoundedContext, "Llamada API a");
            springBootApi.Uses(reportBoundedContext, "Llamada API a");
            springBootApi.Uses(paymentBoundedContext, "Llamada API a");
            springBootApi.Uses(applianceCategoryBoundedContext, "Llamada API a");

            appointmentBoundedContext.Uses(dataBase, "Lee desde y Escribe a");
            publicationBoundedContext.Uses(dataBase, "Lee desde y Escribe a");
            technicalBoundedContext.Uses(dataBase, "Lee desde y Escribe a");
            userBoundedContext.Uses(dataBase, "Lee desde y Escribe a");
            reportBoundedContext.Uses(dataBase, "Lee desde y Escribe a");
            paymentBoundedContext.Uses(dataBase, "Lee desde y Escribe a");
            applianceCategoryBoundedContext.Uses(dataBase, "Lee desde y Escribe a");

            stripeApi.Uses(paymentBoundedContext, "Permite los pagos por los servicios ofrecidos");

            //Tags
            webApplication.AddTags("WebApp");
            singlePageApplication.AddTags("PageApp");
            springBootApi.AddTags("SpringAPI");
            
            appointmentBoundedContext.AddTags("Appointment");
            publicationBoundedContext.AddTags("Publication");
            technicalBoundedContext.AddTags("TechnicalBC");
            userBoundedContext.AddTags("UserBC");
            reportBoundedContext.AddTags("Report");
            paymentBoundedContext.AddTags("Payment");
            applianceCategoryBoundedContext.AddTags("Category");
            dataBase.AddTags("DataBase");
            
            //Styles
            styles.Add(new ElementStyle("WebApp") { Background = "#15ab92", Color = "#ffffff", Shape = Shape.WebBrowser});
            styles.Add(new ElementStyle("PageApp") { Background = "#9acd32", Color = "#ffffff", Shape = Shape.RoundedBox});
            styles.Add(new ElementStyle("SpringAPI") { Background = "#00276c", Color = "#ffffff", Shape = Shape.RoundedBox});
            
            styles.Add(new ElementStyle("Appointment") { Background = "#ff9800", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("Publication") { Background = "#ff9800", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("TechnicalBC") { Background = "#ff9800", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("UserBC") { Background = "#ff9800", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("Report") { Background = "#ff9800", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("Payment") { Background = "#ff9800", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("Category") { Background = "#ff9800", Color = "#ffffff", Shape = Shape.Hexagon });    
            styles.Add(new ElementStyle("DataBase") { Background = "#E00000", Color = "#ffffff", Shape = Shape.Cylinder });

            ContainerView containerView = viewSet.CreateContainerView(safePlatform, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A3_Landscape;
            containerView.AddAllElements();  
            
            //Diagrama de componentes Appointment BC
            Component appointmentController = appointmentBoundedContext.AddComponent("Appointment Controller", "Controlador que provee los Rest API para la gestión de citas", "");
            Component appointmentService = appointmentBoundedContext.AddComponent("Appointment Service", "Provee los métodos para la inscripción y gestión de citas", "");
            Component appointmentRepository = appointmentBoundedContext.AddComponent("Appointment Repository", "Repositorio que provee los métodos para la persistencia de los datos de las citas.", "");
            Component appointmentDomain = appointmentBoundedContext.AddComponent("Appointment Domain Model", "Contiene todas las entidades del Bounded Context", "");

            springBootApi.Uses(appointmentController, "Llamada API");
            appointmentController.Uses(appointmentService, "Llamada a los métodos del service");
            appointmentService.Uses(appointmentRepository, "Llamada a los métodos de persistencia del repository");
            appointmentDomain.Uses(appointmentRepository, "Conforma");
            appointmentRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            //Tags
            appointmentController.AddTags("appointmentController");
            appointmentService.AddTags("appointmentService");
            appointmentRepository.AddTags("appointmentRepository");
            appointmentDomain.AddTags("appointmentDomain");
            
            styles.Add(new ElementStyle("appointmentController") {Background = "#F58900", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("appointmentService") {Background = "#F58900", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("appointmentRepository") {Background = "#F58900", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("appointmentDomain") {Background = "#F58900", Color = "#ffffff", Shape = Shape.Component});

            ComponentView componentView = viewSet.CreateComponentView(appointmentBoundedContext, "Components", "Component Diagram");
            componentView.PaperSize = PaperSize.A4_Landscape;
            componentView.Add(appointmentBoundedContext);
            componentView.Add(springBootApi);
            componentView.Add(singlePageApplication);
            componentView.Add(dataBase);
            componentView.AddAllComponents();
           
            //Diagrama de componentes Publication BC
            
            Component publicationController = publicationBoundedContext.AddComponent("Publication Controller", "Controlador que provee los Rest API para la gestión de publiaciones", "");
            Component publicationService = publicationBoundedContext.AddComponent("Publication Service", "Provee los métodos para la gestión de publicaciones", "");
            Component publicationRepository = publicationBoundedContext.AddComponent("Publication Repository", "Repositorio que provee los métodos para la persistencia de los datos de las publicaciones.", "");
            Component publicationDomain = publicationBoundedContext.AddComponent("Publication Domain Model", "Contiene todas las entidades del Bounded Context", "");
            
            springBootApi.Uses(publicationController, "Llamada API");
            publicationController.Uses(publicationService, "Llamada a los métodos del service");
            publicationService.Uses(publicationRepository, "Llamada a los métodos de persistencia del repository");
            publicationDomain.Uses(publicationRepository, "Conforma");
            publicationRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            //Tags
            publicationController.AddTags("publicationController");
            publicationService.AddTags("publicationService");
            publicationRepository.AddTags("publicationRepository");
            publicationDomain.AddTags("publicationDomain");
            
            styles.Add(new ElementStyle("publicationController") {Background = "#760000", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("publicationService") {Background = "#760000", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("publicationRepository") {Background = "#760000", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("publicationDomain") {Background = "#760000", Color = "#ffffff", Shape = Shape.Component});

            ComponentView componentView2 = viewSet.CreateComponentView(publicationBoundedContext, "Components2", "Component Diagram");
            componentView2.PaperSize = PaperSize.A4_Landscape;
            componentView2.Add(publicationBoundedContext);
            componentView2.Add(springBootApi);
            componentView2.Add(singlePageApplication);
            componentView2.Add(dataBase);
            componentView2.AddAllComponents();
            
            //Diagrama de componentes Technical BC
            Component technicalController = technicalBoundedContext.AddComponent("Technical Controller", "Provee las RestAPI para el manejo de técnicos");
            Component technicalService = technicalBoundedContext.AddComponent("Technical Service", "Provee los métodos para la inscripción y gestión de técnicos");
            Component technicalRepository = technicalBoundedContext.AddComponent("Technical Repository", "Provee los métodos para la persistencia de los datos de los técnicos");
            Component technicalDomain = technicalBoundedContext.AddComponent("Technical Domain Model", "Contiene todas las entidades del Bounded Context");
            Component technicalValidation = technicalBoundedContext.AddComponent("Technical Validation", "Se encarga de validar que los datos del técnico son los correctos");
            
            springBootApi.Uses(technicalController, "Llamada API");
            technicalController.Uses(technicalService, "Llamada a los métodos del service");
            technicalService.Uses(technicalRepository, "Llamada a los métodos de persistencia del repository");
            technicalService.Uses(technicalValidation, "Llamada a los métodos de validación");
            technicalDomain.Uses(technicalRepository, "Conforma");
            technicalRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            //Tags
            technicalController.AddTags("technicalController");
            technicalService.AddTags("technicalService");
            technicalRepository.AddTags("technicalRepository");
            technicalDomain.AddTags("technicalDomain");
            technicalValidation.AddTags("technicalValidation");
            
            styles.Add(new ElementStyle("technicalController") {Background = "#6D4C41", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("technicalService") {Background = "#6D4C41", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("technicalRepository") {Background = "#6D4C41", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("technicalDomain") {Background = "#6D4C41", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("technicalValidation") {Background = "#6D4C41", Color = "#ffffff", Shape = Shape.Component});
            
            ComponentView componentView3 = viewSet.CreateComponentView(technicalBoundedContext, "Components3", "Component Diagram");
            componentView3.PaperSize = PaperSize.A4_Landscape;
            componentView3.Add(technicalBoundedContext);
            componentView3.Add(singlePageApplication);
            componentView3.Add(springBootApi);
            componentView3.Add(dataBase);
            componentView3.AddAllComponents();
            
            //Diagrama de componentes User BC
            Component userController = userBoundedContext.AddComponent("User Controller", "Provee las RestAPI para el manejo de usuarios");
            Component userService = userBoundedContext.AddComponent("User Service", "Provee los métodos para la inscripción y gestión de usuarios");
            Component userRepository = userBoundedContext.AddComponent("User Repository", "Provee los métodos para la persistencia de los datos de los usuarios");
            Component userDomain = userBoundedContext.AddComponent("User Domain Model", "Contiene todas las entidades del Bounded Context");
            Component userValidation = userBoundedContext.AddComponent("User Validation", "Se encarga de validar que los datos del usuario son los correctos");
            
            springBootApi.Uses(userController, "Llamada API");
            userController.Uses(userService, "Llamada a los métodos del service");
            userService.Uses(userRepository, "Llamada a los métodos de persistencia del repository");
            userService.Uses(userValidation, "Llamada a los métodos de validación");
            userDomain.Uses(userRepository, "Conforma");
            userRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            //Tags
            userController.AddTags("userController");
            userService.AddTags("userService");
            userRepository.AddTags("userRepository");
            userDomain.AddTags("userDomain");
            userValidation.AddTags("userValidation");
            
            styles.Add(new ElementStyle("userController") {Background = "#50126D", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("userService") {Background = "#50126D", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("userRepository") {Background = "#50126D", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("userDomain") {Background = "#50126D", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("userValidation") {Background = "#50126D", Color = "#ffffff", Shape = Shape.Component});
            
            ComponentView componentView4 = viewSet.CreateComponentView(userBoundedContext, "Components4", "Component Diagram");
            componentView4.PaperSize = PaperSize.A4_Landscape;
            componentView4.Add(userBoundedContext);
            componentView4.Add(singlePageApplication);
            componentView4.Add(springBootApi);
            componentView4.Add(dataBase);
            componentView4.AddAllComponents();
            
            //Diagrama de componentes Payment BC
            Component paymentController = paymentBoundedContext.AddComponent("Payment Controller", "Controlador que provee los RestAPI para la gestión de pagos", "");
            Component paymentService = paymentBoundedContext.AddComponent("Payment Service", "Provee los métodos para la realización de los pagos", "");
            Component stripeService = paymentBoundedContext.AddComponent("Stripe Service", "Servicio encargado de conectarse con la plataforma de PayPal para verificar el pago del cliente", "");
            Component paymentRepository = paymentBoundedContext.AddComponent("Payment Repository", "Provee los métodos para la persistencia de los datos de los pagos", "");
            Component paymentDomain = paymentBoundedContext.AddComponent("Payment Domain Model", "Contiene todas las entidades del Bounded Context", "");
            
            springBootApi.Uses(paymentController, "Llamada API a");
            paymentController.Uses(paymentService, "Llamada a los métodos del service");
            paymentController.Uses(stripeService, "Llamada a los métodos del service");
            stripeService.Uses(stripeApi, "Usa");
            paymentService.Uses(paymentRepository, "Llamada a los métodos de persistencia del repository");
            paymentDomain.Uses(paymentRepository, "Conforma");
            paymentRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            //Tags
            paymentController.AddTags("paymentController");
            paymentService.AddTags("paymentService");
            stripeService.AddTags("stripeService");
            paymentRepository.AddTags("paymentRepository");
            paymentDomain.AddTags("paymentDomain");
            
            styles.Add(new ElementStyle("paymentController") {Background = "#34FF2C", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("paymentService") {Background = "#34FF2C", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("stripeService") {Background = "#34FF2C", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("paymentRepository") {Background = "#34FF2C", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("paymentDomain") {Background = "#34FF2C", Color = "#ffffff", Shape = Shape.Component});
            
            ComponentView componentView5 = viewSet.CreateComponentView(paymentBoundedContext, "Components5", "Component Diagram");
            componentView5.PaperSize = PaperSize.A4_Landscape;
            componentView5.Add(paymentBoundedContext);
            componentView5.Add(singlePageApplication);
            componentView5.Add(springBootApi);
            componentView5.Add(stripeApi);
            componentView5.Add(dataBase);
            componentView5.AddAllComponents();
            
            //Diagrama de componentes Report BC
            
            Component reportController = reportBoundedContext.AddComponent("Report Controller", "Controlador que provee los Rest API para la gestión de reportes", "");
            Component reportService = reportBoundedContext.AddComponent("Report Service", "Provee los métodos para la gestión de reportes", "");
            Component reportRepository = reportBoundedContext.AddComponent("Report Repository", "Repositorio que provee los métodos para la persistencia de los datos de los reportes.", "");
            Component reportDomain = reportBoundedContext.AddComponent("Report Domain Model", "Contiene todas las entidades del Bounded Context", "");
            
            springBootApi.Uses(reportController, "Llamada API");
            reportController.Uses(reportService, "Llamada a los métodos del service");
            reportService.Uses(reportRepository, "Llamada a los métodos de persistencia del repository");
            reportDomain.Uses(reportRepository, "Conforma");
            reportRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            //Tags
            reportController.AddTags("reportController");
            reportService.AddTags("reportService");
            reportRepository.AddTags("reportRepository");
            reportDomain.AddTags("reportDomain");
            
            styles.Add(new ElementStyle("reportController") {Background = "#18FFFF", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("reportService") {Background = "#18FFFF", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("reportRepository") {Background = "#18FFFF", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("reportDomain") {Background = "#18FFFF", Color = "#ffffff", Shape = Shape.Component});

            ComponentView componentView6 = viewSet.CreateComponentView(reportBoundedContext, "Components6", "Component Diagram");
            componentView6.PaperSize = PaperSize.A4_Landscape;
            componentView6.Add(reportBoundedContext);
            componentView6.Add(springBootApi);
            componentView6.Add(singlePageApplication);
            componentView6.Add(dataBase);
            componentView6.AddAllComponents();
            
            //Diagrama de componentes Appliance Category BC
            
            Component applianceCategoryController = applianceCategoryBoundedContext.AddComponent("Appliance Category Controller", "Controlador que provee los Rest API para la gestión de categorías de electrodomésticos", "");
            Component applianceCategoryService = applianceCategoryBoundedContext.AddComponent("Appliance Category Service", "Provee los métodos para la gestión de categorías de electrodomésticos", "");
            Component applianceCategoryRepository = applianceCategoryBoundedContext.AddComponent("Appliance Category Repository", "Repositorio que provee los métodos para la persistencia de los datos de las categorías de los electrodomésticos.", "");
            Component applianceCategoryDomain = applianceCategoryBoundedContext.AddComponent("Appliance Category Domain Model", "Contiene todas las entidades del Bounded Context", "");

            springBootApi.Uses(applianceCategoryController, "Llamada API");
            applianceCategoryController.Uses(applianceCategoryService, "Llamada a los métodos del service");
            applianceCategoryService.Uses(applianceCategoryRepository, "Llamada a los métodos de persistencia del repository");
            applianceCategoryDomain.Uses(applianceCategoryRepository, "Conforma");
            applianceCategoryRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            //Tags
            applianceCategoryController.AddTags("applianceCategoryController");
            applianceCategoryService.AddTags("applianceCategoryService");
            applianceCategoryRepository.AddTags("applianceCategoryRepository");
            applianceCategoryDomain.AddTags("applianceCategoryDomain");
            
            styles.Add(new ElementStyle("applianceCategoryController") {Background = "#FFEA00", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("applianceCategoryService") {Background = "#FFEA00", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("applianceCategoryRepository") {Background = "#FFEA00", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("applianceCategoryDomain") {Background = "#FFEA00", Color = "#ffffff", Shape = Shape.Component});

            ComponentView componentView7 = viewSet.CreateComponentView(applianceCategoryBoundedContext, "Components7", "Component Diagram");
            componentView7.PaperSize = PaperSize.A4_Landscape;
            componentView7.Add(applianceCategoryBoundedContext);
            componentView7.Add(springBootApi);
            componentView7.Add(singlePageApplication);
            componentView7.Add(dataBase);
            componentView7.AddAllComponents();
            
            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}