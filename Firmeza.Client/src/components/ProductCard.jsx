import { useCart } from '../contexts/CartContext';
import { FiShoppingCart, FiCheck } from 'react-icons/fi';
import { useState } from 'react';
import './ProductCard.css';

const ProductCard = ({ product }) => {
    const { addToCart } = useCart();
    const [added, setAdded] = useState(false);

    const handleAddToCart = () => {
        addToCart(product, 1);
        setAdded(true);
        setTimeout(() => setAdded(false), 2000);
    };

    return (
        <div className="product-card card">
            <div className="product-image">
                <div className="product-placeholder">
                    {product.name.charAt(0)}
                </div>
                {product.stock < 10 && product.stock > 0 && (
                    <div className="badge low-stock-badge">Low Stock</div>
                )}
                {product.stock === 0 && (
                    <div className="badge out-of-stock-badge">Out of Stock</div>
                )}
            </div>

            <div className="product-content">
                <h3 className="product-name">{product.name}</h3>
                {product.category && (
                    <span className="product-category">{product.category}</span>
                )}
                <p className="product-description">
                    {product.description || 'No description available'}
                </p>

                <div className="product-footer">
                    <div className="product-price">${product.price.toFixed(2)}</div>
                    <button
                        className={`btn btn-sm ${added ? 'btn-secondary' : 'btn-primary'}`}
                        onClick={handleAddToCart}
                        disabled={product.stock === 0 || added}
                    >
                        {added ? (
                            <>
                                <FiCheck /> Added
                            </>
                        ) : (
                            <>
                                <FiShoppingCart /> Add to Cart
                            </>
                        )}
                    </button>
                </div>
            </div>
        </div>
    );
};

export default ProductCard;
