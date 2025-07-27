import React, { createContext, useState, useEffect } from 'react';
import api from '../services/api';

export const CartContext = createContext();

export const CartProvider = ({ children }) => {
  const [cart, setCart] = useState([]);
  const [total, setTotal] = useState(0);

  const fetchCart = async () => {
    try {
      const res = await api.get('/cart');
      setCart(res.data);
      setTotal(res.data.reduce((sum, item) => sum + (item.gift?.price || item.price || 0) * item.quantity, 0));
    } catch {
      setCart([]);
      setTotal(0);
    }
  };

  useEffect(() => {
    fetchCart();
  }, []);

  const removeFromCart = async (itemId) => {
    await api.delete(`/cart/${itemId}`);
    fetchCart();
  };

  return (
    <CartContext.Provider value={{ cart, removeFromCart, total, fetchCart }}>
      {children}
    </CartContext.Provider>
  );
};
