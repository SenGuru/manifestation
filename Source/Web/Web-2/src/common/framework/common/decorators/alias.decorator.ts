
const REGISTRY = new Map<string, Map<string, string[]>>();

function register(cls: string, property: string, decorator: string): void {
  let map: Map<string, string[]> | undefined;

  if (REGISTRY.has(cls)) {
    map = REGISTRY.get(cls);
  } else {
    map = new Map<string, string[]>();
    REGISTRY.set(cls, map);
  }

  let list: string[] | undefined;
  if (map?.has(property)) {
    list = map.get(property);
  } else {
    list = [];
    map?.set(property, list);
  }

  if (list?.indexOf(decorator) ?? 0 < 0) {
    list?.push(decorator);
  }
}

export function alias(name: string) {
  // eslint-disable-next-line
  return function (cls: any, property: string) {
    // eslint-disable-next-line
    register(cls.constructor.name, property, name);
  };
}

// eslint-disable-next-line
export function getListOfAttributeDecorators(obj: any, property: string): string[] | undefined {
  // eslint-disable-next-line
  const name = obj.constructor.name;
  // eslint-disable-next-line
  return !REGISTRY.has(name) ? [] : REGISTRY?.get(name)?.get(property);
}

/*
function alias(name: string): any {
  return function DoAliasProperty(target: any, propertyKey: string, descriptor: PropertyDescriptor) {
      descriptor.get = function () {
          if (ObjectUtils.hasKey(this, name)) {
            return this.data[name];
          } else {
            return this.data[propertyKey];
          }
      };

      descriptor.set = function (value) {
        if (ObjectUtils.hasKey(this, name)) {
          this.data[name] = value;
        } else {
          this.data[propertyKey] = value;
        }
      };
  };
}
*/
