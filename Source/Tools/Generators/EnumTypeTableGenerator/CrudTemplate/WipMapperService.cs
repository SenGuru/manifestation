public partial List<EEntity123Model> EEntity123ViewListToEEntity123ModelList(List<EEntity123View> lst);
public partial List<EEntity123Model> EEntity123ListToEEntity123ModelList(List<EEntity123> lst);
public partial EEntity123Model EEntity123ViewToEEntity123Model(EEntity123View vw);
[MapperRequiredMapping(RequiredMappingStrategy.None)]
public partial EEntity123Model EEntity123ToEEntity123Model(EEntity123 model);
public partial EEntity123 EEntity123CreateModelToEEntity123(EEntity123CreateModel model);
public partial EEntity123 EEntity123UpdateModelToEEntity123(EEntity123UpdateModel model);

//public partial List<EEntity123ExportModel> EEntity123ModelListToEEntity123ExportModelList(List<EEntity123Model> lst);