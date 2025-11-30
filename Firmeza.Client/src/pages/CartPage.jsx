import { Link } from 'react-router-dom';
import { useCart } from '../contexts/CartContext';
import { FiTrash2, FiPlus, FiMinus, FiShoppingCart } from 'react-icons/fi';
import './CartPage.css';

const CartPage = () => {
    const { cart, removeFromCart, updateQuantity, getSubtotal, getTax, getTotal } = useCart();

    if (cart.length === 0) {
        return (
            <div className="cart-page">
                <div className="container">
                    <div className="empty-cart">
                        <FiShoppingCart size={80} />
                        <h2>Your cart is empty</h2>
                        <p>Add some products to get started!</p>
                        <Link to="/products" className="btn btn-primary">
                            Browse Products
                        </Link>
                    </div>
                </div>
            </div>
        );
    }

    return (
        <div className="cart-page">
            <div className="container">
                <h1>Shopping Cart</h1>

                <div className="cart-layout">
                    <div className="cart-items">
                        {cart.map((item) => (
                            <div key={item.id} className="cart-item card">
                                <div className="cart-item-image">
                                    <div className="product-placeholder">
                                        {item.name.charAt(0)}
                                    </div>
                                </div>

                                <div className="cart-item-details">
                                    <h3>{item.name}</h3>
                                    {item.category && (
                                        <span className="product-category">{item.category}</span>
                                    )}
                                    <p className="cart-item-price">${item.price.toFixed(2)} each</p>
                                </div>

                                <div className="cart-item-actions">
                                    <div className="quantity-controls">
                                        <button
                                            className="btn btn-sm btn-outline"
                                            onClick={() => updateQuantity(item.id, item.quantity - 1)}
                                        >
                                            <FiMinus />
                                        </button>
                                        <span className="quantity">{item.quantity}</span>
                                        <button
                                            className="btn btn-sm btn-outline"
                                            onClick={() => updateQuantity(item.id, item.quantity + 1)}
                                            disabled={item.quantity >= item.stock}
                                        >
                                            <FiPlus />
                                        </button>
                                    </div>

                                    <div className="cart-item-total">
                                        ${(item.price * item.quantity).toFixed(2)}
                                    </div>

                                    <button
                                        className="btn btn-sm btn-outline delete-btn"
                                        onClick={() => removeFromCart(item.id)}
                                        title="Remove from cart"
                                    >
                                        <FiTrash2 />
                                    </button>
                                </div>
                            </div>
                        ))}
                    </div>

                    <div className="cart-summary card card-glass">
                        <h2>Order Summary</h2>

                        <div className="summary-row">
                            <span>Subtotal:</span>
                            <span>${getSubtotal().toFixed(2)}</span>
                        </div>

                        <div className="summary-row">
                            <span>Tax (10%):</span>
                            <span>${getTax().toFixed(2)}</span>
                        </div>

                        <div className="summary-divider"></div>

                        <div className="summary-row summary-total">
                            <span>Total:</span>
                            <span>${getTotal().toFixed(2)}</span>
                        </div>

                        <Link to="/checkout" className="btn btn-primary btn-lg" style={{ width: '100%' }}>
                            Proceed to Checkout
                        </Link>

                        <Link to="/products" className="btn btn-outline" style={{ width: '100%' }}>
                            Continue Shopping
                        </Link>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default CartPage;
