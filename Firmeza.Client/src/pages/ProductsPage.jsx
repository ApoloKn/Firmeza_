import { useState, useEffect } from 'react';
import { getProducts } from '../api/productsApi';
import ProductCard from '../components/ProductCard';
import './ProductsPage.css';

const ProductsPage = () => {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [page, setPage] = useState(1);
    const [searchTerm, setSearchTerm] = useState('');

    useEffect(() => {
        loadProducts();
    }, [page]);

    const loadProducts = async () => {
        try {
            setLoading(true);
            const data = await getProducts(page, 12);
            setProducts(data);
            setError('');
        } catch (err) {
            setError('Failed to load products');
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    const filteredProducts = products.filter(product =>
        product.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        product.description?.toLowerCase().includes(searchTerm.toLowerCase()) ||
        product.category?.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <div className="products-page">
            <div className="container">
                <div className="products-header">
                    <h1>Our Products</h1>
                    <p>Discover amazing products at great prices</p>
                </div>

                <div className="products-controls">
                    <input
                        type="text"
                        className="form-input search-input"
                        placeholder="Search products..."
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                    />
                </div>

                {error && (
                    <div className="alert alert-error">
                        {error}
                    </div>
                )}

                {loading ? (
                    <div className="flex flex-center" style={{ minHeight: '400px' }}>
                        <div className="spinner-lg"></div>
                    </div>
                ) : (
                    <>
                        {filteredProducts.length === 0 ? (
                            <div className="no-products">
                                <h3>No products found</h3>
                                <p>Try adjusting your search terms</p>
                            </div>
                        ) : (
                            <div className="products-grid grid grid-3 fade-in">
                                {filteredProducts.map((product) => (
                                    <ProductCard key={product.id} product={product} />
                                ))}
                            </div>
                        )}

                        {!searchTerm && products.length >= 12 && (
                            <div className="pagination">
                                <button
                                    className="btn btn-outline"
                                    onClick={() => setPage(p => Math.max(1, p - 1))}
                                    disabled={page === 1}
                                >
                                    Previous
                                </button>
                                <span className="page-info">Page {page}</span>
                                <button
                                    className="btn btn-outline"
                                    onClick={() => setPage(p => p + 1)}
                                    disabled={products.length < 12}
                                >
                                    Next
                                </button>
                            </div>
                        )}
                    </>
                )}
            </div>
        </div>
    );
};

export default ProductsPage;
