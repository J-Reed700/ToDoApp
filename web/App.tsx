import { Dashboard } from './components';

import { ApiServiceProvider } from './context';

// Global Styles
import './styles.css';

export default function App() {
  return (
    <ApiServiceProvider>
      <Dashboard />
    </ApiServiceProvider>
  );
}
