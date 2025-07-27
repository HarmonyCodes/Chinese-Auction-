import React, { useContext } from 'react';
import { Link } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';

const NavBar = () => {
  const { user, logout } = useContext(AuthContext);
  const role = user?.role;

  return (
    <nav style={{display:'flex', gap:'1rem', marginBottom:'1rem'}}>
      <Link to="/">דף הבית</Link>
      {!user && <Link to="/login">התחברות</Link>}
      {!user && <Link to="/register">הרשמה</Link>}
      {user && <Link to="/cart">עגלת קניות</Link>}
      {user && <Link to="/purchases">הרכישות שלי</Link>}
      <Link to="/winners">דוח זוכים</Link>
      {role === 'Manager' && <Link to="/admin/donors">ניהול תורמים</Link>}
      {role === 'Manager' && <Link to="/admin/gifts">ניהול מתנות</Link>}
      {role === 'Manager' && <Link to="/admin/purchases">ניהול רכישות</Link>}
      {role === 'Manager' && <Link to="/admin/reports">דוחות</Link>}
      {user && <button onClick={logout}>התנתק</button>}
    </nav>
  );
};

export default NavBar;
