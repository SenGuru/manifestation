export class JsonUtils {
   /**
 * Safely stringify a value with circular references
 * @template T The type of the value being stringified
 * @param value The value to stringify
 * @param options Stringify options
 * @returns A JSON string representation with circular references handled
 */
public static safeStringify<T>(
    value: T, 
    options: { 
      spaces?: number; 
      circularValue?: string; 
    } = {}
  ): string {
    const { spaces, circularValue = '[Circular]' } = options;
    const seen = new WeakSet();
    
    const replacer = (_key: string, val: unknown): unknown => {
      if (typeof val === 'object' && val !== null) {
        if (seen.has(val as object)) {
          return circularValue;
        }
        seen.add(val as object);
      }
      return val;
    };
    
    return JSON.stringify(value, replacer, spaces);
  }

}