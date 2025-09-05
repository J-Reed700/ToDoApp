const API_BASE_URL = 'http://localhost:5000/api';


export abstract class BaseApiService {
  
  protected async request<T>(endpoint: string, options: RequestInit & { expectResponse: true }): Promise<T>;
  protected async request(endpoint: string, options?: RequestInit & { expectResponse?: false }): Promise<void>;
  
  protected async request<T = void>(
    endpoint: string, 
    options: RequestInit & { expectResponse?: boolean } = {}
  ): Promise<T | void> {
    const { expectResponse = false, ...fetchOptions } = options;
    
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      headers: {
        'Content-Type': 'application/json',
        ...fetchOptions.headers,
      },
      ...fetchOptions,
    });

    if (!response.ok) {
      throw new Error(`API Error: ${response.status} ${response.statusText}`);
    }

    if (expectResponse) {
      const contentType = response.headers.get('content-type');
      const contentLength = response.headers.get('content-length');
      
      if (contentLength === '0' || !contentType?.includes('application/json')) {
        return null as T;
      }

      const text = await response.text();
      if (!text || text.trim() === '') {
        return null as T;
      }

      try {
        return JSON.parse(text) as T;
      } catch (error) {
        console.warn('Failed to parse JSON response:', error);
        return null as T;
      }
    }

    return;
  }
}
