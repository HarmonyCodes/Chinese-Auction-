import React, { useContext } from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import Login from './pages/Login';
import Register from './pages/Register';
import CartView from './components/CartView';
import AddGiftToCart from './components/AddGiftToCart';
import WinnersReport from './components/WinnersReport';
import AdminDonors from './pages/AdminDonors';
import AdminGifts from './pages/AdminGifts';
import AdminPurchases from './pages/AdminPurchases';
import AdminReports from './pages/AdminReports';
import NavBar from './components/NavBar';
import { AuthProvider, AuthContext } from './context/AuthContext';
import { CartProvider } from './context/CartContext';
import Purchases from './pages/Purchases';

const ProtectedRoute = ({ children }) => {
  const { user } = useContext(AuthContext);
  return user ? children : <Navigate to="/login" />;
};

function App() {
  return (
    <AuthProvider>
      <CartProvider>
        <Router>
          <NavBar />
          <Routes>
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/cart" element={
              <ProtectedRoute>
                <CartView />
                <AddGiftToCart />
              </ProtectedRoute>
            } />
            <Route path="/purchases" element={
              <ProtectedRoute>
                <Purchases />
              </ProtectedRoute>
            } />
            <Route path="/winners" element={<WinnersReport />} />
            <Route path="/admin/donors" element={<AdminDonors />} />
            <Route path="/admin/gifts" element={<AdminGifts />} />
            <Route path="/admin/purchases" element={<AdminPurchases />} />
            <Route path="/admin/reports" element={<AdminReports />} />
            <Route path="*" element={<Navigate to="/cart" />} />
          </Routes>
        </Router>
      </CartProvider>
    </AuthProvider>
  );
}

export default App;
