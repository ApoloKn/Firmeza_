import { Link } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { useCart } from '../contexts/CartContext';
import { FiShoppingCart, FiUser, FiLogOut, FiHome, FiPackage } from 'react-icons/fi';
import './Navbar.css';

const Navbar = () => {
    const { user, isAuthenticated, logout } = useAuth();
    const { getItemCount } = useCart();

    return (
        <nav className="navbar">
            <div className="container">
                <div className="navbar-content">
                    <Link to="/" className="navbar-brand">
                        <div className="brand-logo">F</div>
                        <span className="brand-name">Firmeza</span>
                    </Link>

                    <div className="navbar-links">
                        <Link to="/" className="nav-link">
                            <FiHome /> <span>Home</span>
                        </Link>
                        <Link to="/products" className="nav-link">
                            <FiPackage /> <span>Products</span>
                        </Link>
                        <Link to="/cart" className="nav-link cart-link">
                            <FiShoppingCart />
                            {getItemCount() > 0 && (
                                <span className="cart-badge">{getItemCount()}</span>
                            )}
                            <span>Cart</span>
                        </Link>
                    </div>

                    <div className="navbar-actions">
                        {isAuthenticated() ? (
                            <div className="user-menu">
                                <span className="user-name">
                                    <FiUser /> {user?.username}
                                </span>
                                <button onClick={logout} className="btn btn-outline btn-sm">
                                    <FiLogOut /> Logout
                                </button>
                            </div>
                        ) : (
                            <div className="auth-links">
                                <Link to="/login" className="btn btn-outline btn-sm">
                                    Login
                                </Link>
                                <Link to="/register" className="btn btn-primary btn-sm">
                                    Register
                                </Link>
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </nav>
    );
};

export default Navbar;
